using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class Button : MonoBehaviour
    {
        public GameObject door;
        private Door doorScript;
        public bool openDoor;

        // This is bad and TEMP,
        // basically when the player is in this trigger and presses
        // 'E' it activates the airlock cycle, starting the open door process
        // - Edmund
        // 

        void Start()
        {
            doorScript = door.GetComponent<Door>();
        }

        private void OnTriggerStay(Collider other) // while in trigger if 'E' is pressed do the thing
        {
            if (other.tag != "Player") return;
            UI_Manager.ui_Manager.tooltipActiveElsewhere = true;
            UI_Manager.ui_Manager.ActivatePrimaryInteractText();
            if (InputManager.inputManager.ePressed)
            {
                if (doorScript.isDoorOpening) return;
                if (doorScript.otherDoor != null)
                {
                    if (doorScript.otherDoor.isDoorOpening) return;
                }
                if (!openDoor)
                {
                    Debug.LogWarning("Press");
                    doorScript.OpenDoor();
                    openDoor = true;
                }
                else if (!doorScript.noLoad)
                {
                    if (!openDoor)
                    {
                        Debug.LogWarning("Press");
                        doorScript.OpenDoor();
                        openDoor = true;                    
                    }
                    else
                    {
                        Debug.LogWarning("LoadScene");
                        Level_Manager.LM.LoadOutside();
                        openDoor = false;
                    }
                }
                else
                {
                    openDoor = false;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag != "Player") return;
            UI_Manager.ui_Manager.tooltipActiveElsewhere = false;
            UI_Manager.ui_Manager.DisablePrimaryInteractText();
        }
    }

}
