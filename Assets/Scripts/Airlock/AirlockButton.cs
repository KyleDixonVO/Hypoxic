using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class AirlockButton : Interactable
    {
        public GameObject Door;
        [SerializeField]
        Airlock AL;

        public override void OnInteract()
        {
            Debug.Log("Button pressed");
            if (this.GetComponent<AudioSource>().isPlaying == false)
            {
                AudioManager.audioManager.PlaySound(this.GetComponent<AudioSource>(), AudioManager.audioManager.doorButton);
            }
            AL.OpenDoor();
            AL.isOpening = true;
        }

        public override void OnFocus()
        {
            UI_Manager.ui_Manager.ActivatePrimaryInteractText();
        }

        public override void OnLoseFocus()
        {
            UI_Manager.ui_Manager.DisablePrimaryInteractText();
        }
    }

}
