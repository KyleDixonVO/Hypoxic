using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror 
{
    public class SaveStation : Interactable
    {
        [SerializeField] string tooltip;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnFocus()
        {
            UI_Manager.ui_Manager.ActivatePrimaryInteractText(tooltip);
        }

        public override void OnLoseFocus()
        {
            UI_Manager.ui_Manager.DisablePrimaryInteractText();
        }

        public override void OnInteract()
        {
            if (Data_Manager.dataManager.saving) return;
            GameManager.gameManager.SaveGame();
            Debug.LogWarning("GameSaved");
        }
    }
}


