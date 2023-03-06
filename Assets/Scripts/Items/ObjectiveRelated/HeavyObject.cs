using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class HeavyObject : Interactable
    {
        [SerializeField] private bool _isHeld = false;
        public bool isHeld;
        [SerializeField] private Vector3 initialPos;
        [SerializeField] private Vector3 heldPos;
        [SerializeField] private Vector3 heldRot;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            UpdateObjectParent();
            UpdateHeldLocalPosition();
            isHeld = _isHeld;
        }

        // Interact system
        public override void OnInteract()
        {
            if (WithinPickupRange()
                && !_isHeld
                && FirstPersonController_Sam.fpsSam.carryingHeavyObj == false)
            {
                Debug.Log("Picked up heavy object");
                AudioManager.audioManager.PlaySound(this.gameObject.GetComponent<AudioSource>(), AudioManager.audioManager.pickupPipe);
                _isHeld = true;
                FirstPersonController_Sam.fpsSam.carryingHeavyObj = true;
            }
            else if (_isHeld)
            {
                Debug.Log("Dropped heavy object");
                AudioManager.audioManager.PlaySound(this.gameObject.GetComponent<AudioSource>(), AudioManager.audioManager.dropPipe);
                _isHeld = false;
                FirstPersonController_Sam.fpsSam.carryingHeavyObj = false;
            }
        }

        public override void OnFocus()
        {
            Debug.LogWarning("Looking at pipe");
            UI_Manager.ui_Manager.ActivatePrimaryInteractText();
        }

        public override void OnLoseFocus()
        {
            UI_Manager.ui_Manager.DisablePrimaryInteractText();
            //Debug.LogWarning("not looking at pipe");
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
                this.gameObject.transform.rotation = transform.parent.transform.localRotation;
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

        public void ResetToStartingPosition()
        {
            this.transform.position = initialPos;
        }
    }
}
