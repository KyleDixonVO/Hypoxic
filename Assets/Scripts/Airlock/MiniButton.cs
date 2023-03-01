using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class MiniButton : Interactable
    {
        public AirlockMini door;

        public override void OnInteract()
        {
            door.buttonPress = true;
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

