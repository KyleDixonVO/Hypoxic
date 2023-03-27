using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnderwaterHorror 
{
    public class CrosshairController : MonoBehaviour
    {
        [SerializeField] Color normal;
        [SerializeField] Color target;
        [SerializeField] Camera cam;
        [SerializeField] Image crosshair;

        float range;
        private int defaultRange = 5;

        // Start is called before the first frame update
        void Start()
        {
            crosshair.color = normal;
        }

        // Update is called once per frame
        void Update()
        {
            GetCurrentWeapon();

            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
            {
                if (hit.transform.gameObject.layer == 8)
                {
                    crosshair.color = target;
                }
                else if (hit.transform.gameObject.layer == 10) 
                {
                    // can also change color if in range for interactable
                    crosshair.color = normal;
                }
                else
                {
                    crosshair.color = normal;
                }
            }
        }

        void GetCurrentWeapon()
        {
            if (PlayerInventory.playerInventory.inventory[PlayerInventory.playerInventory.activeWeapon] == null) return;

            if (PlayerInventory.playerInventory.inventory[PlayerInventory.playerInventory.activeWeapon].GetComponent<Weapon>())
            {
                range = PlayerInventory.playerInventory.inventory[PlayerInventory.playerInventory.activeWeapon].GetComponent<Weapon>().range;
            }
            else range = defaultRange; // can and probably should be interactable range
        }

        public void ResetCrosshair()
        {
            crosshair.color = normal;
        }
    }
}
