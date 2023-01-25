using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class HeavyObject : MonoBehaviour
    {
        [SerializeField] private bool _isHeld = false;
        public bool isHeld;
        [SerializeField] private Vector3 heldPos;
        [SerializeField] private Vector3 heldRot;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(Vector3.Distance(this.transform.position, FirstPersonController_Sam.fpsSam.transform.position));
            ToggleObjectPickup();
            UpdateObjectParent();
            UpdateHeldLocalPosition();
            isHeld = _isHeld;
        }

        void ToggleObjectPickup()
        {
            //Debug.Log(InputManager.inputManager.eCycled);
            if (InputManager.inputManager.eCycled == false) return;
            if (WithinPickupRange()
                && InputManager.inputManager.ePressed
                && !_isHeld
                && FirstPersonController_Sam.fpsSam.carryingHeavyObj == false)
            {
                Debug.Log("Picked up heavy object");
                _isHeld = true;
                FirstPersonController_Sam.fpsSam.carryingHeavyObj = true;
                //InputManager.inputManager.eCycled = false;
            }
            else if (_isHeld && InputManager.inputManager.ePressed)
            {
                Debug.Log("Dropped heavy object");
                _isHeld = false;
                FirstPersonController_Sam.fpsSam.carryingHeavyObj = false;
                //InputManager.inputManager.eCycled = false;
            }
        
        }

        void UpdateObjectParent()
        {
            if (_isHeld && this.gameObject.transform.parent == null)
            {
                this.gameObject.transform.parent = FirstPersonController_Sam.fpsSam.transform;
            }
            else if (!_isHeld)
            {
                this.gameObject.transform.parent = null;
            }
        }

        void UpdateHeldLocalPosition()
        {
            if (this.gameObject.transform.parent != null)
            {
                this.gameObject.transform.localPosition = heldPos;
                this.gameObject.transform.eulerAngles = heldRot;
            }
        }

       public void ForceDropObject()
       {
            FirstPersonController_Sam.fpsSam.carryingHeavyObj = false;
            //InputManager.inputManager.eCycled = true;
            _isHeld = false;
            UpdateObjectParent();
       }

        public bool WithinPickupRange()
        {
            if (Vector3.Distance(this.transform.position, FirstPersonController_Sam.fpsSam.transform.position) < 2.5) return true;
            return false;
        }
    }
}
