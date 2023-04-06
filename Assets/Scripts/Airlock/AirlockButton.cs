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

        [SerializeField] protected Outline _outline;

        private void Start()
        {
            _outline = GetComponent<Outline>();
            _outline.enabled = false;
        }

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
            if (AL.IsClosed()) AL.isOpening = true;
            AL.timerActive = true;
        }

        public override void OnFocus()
        {
            string tooltip;
            if (AL.isOpenable) tooltip = tooltipOpen;
            else tooltip = tooltipLocked;
            UI_Manager.ui_Manager.ActivatePrimaryInteractText(tooltip);

            if (this.gameObject.GetComponent<Outline>() != null && transform.parent != FirstPersonController_Sam.fpsSam.playerCamera.gameObject)
            {
                //Debug.Log("Setting active outline");
                Outline.activeOutline = gameObject.GetComponent<Outline>();
                _outline.enabled = true;
            }
        }

        public override void OnLoseFocus()
        {
            UI_Manager.ui_Manager.DisablePrimaryInteractText();
            _outline.enabled = false;
        }
    }

}
