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
            AL.OpenDoor();
            AL.isOpening = true;
        }

        public override void OnFocus()
        {
            
        }

        public override void OnLoseFocus()
        {

        }
    }

}
