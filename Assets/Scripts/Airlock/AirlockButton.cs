using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class AirlockButton : Interactable
    {
        public GameObject Door;
        [SerializeField] Airlock AL;
        [SerializeField] string tooltipOpen;
        [SerializeField] string tooltipLocked;

        public override void OnInteract()
        {
            Debug.Log("Button pressed");
            if (this.GetComponent<AudioSource>().isPlaying == false)
            {
                AudioManager.audioManager.PlaySound(this.GetComponent<AudioSource>(), AudioManager.audioManager.doorButton);
                //AL.timeStamp = Time.time;
            }
            //AL.OpenDoor();

            if (!AL.isOpenable) return;
            AL.isOpening = true;
            AL.timerActive = true;
        }

        public override void OnFocus()
        {
            string tooltip;
            if (AL.isOpenable) tooltip = tooltipOpen;
            else tooltip = tooltipLocked;
            UI_Manager.ui_Manager.ActivatePrimaryInteractText(tooltip);
        }

        public override void OnLoseFocus()
        {
            UI_Manager.ui_Manager.DisablePrimaryInteractText();
        }
    }

}
