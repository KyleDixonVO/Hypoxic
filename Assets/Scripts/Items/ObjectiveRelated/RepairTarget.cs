using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror 
{
    public class RepairTarget : Interactable
    {
        public GameObject brokenObject;
        public GameObject repairedObject;
        [SerializeField] string tooltip;

        // Start is called before the first frame update
        void Start()
        {
            BrokenObject();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void RepairedObject()
        {
            repairedObject.GetComponent<MeshRenderer>().enabled = true;
            AudioManager.audioManager.PlaySound(this.gameObject.GetComponent<AudioSource>(), AudioManager.audioManager.repairingComplete);
            brokenObject.SetActive(false);
        }

        public void BrokenObject()
        {
            repairedObject.GetComponent<MeshRenderer>().enabled = false;
            brokenObject.SetActive(true);
        }

        public override void OnFocus()
        {
            if (FirstPersonController_Sam.fpsSam.GetComponentInChildren<RepairObject>() == null) return;
            UI_Manager.ui_Manager.ActivateSecondaryInteractText(tooltip);
        }

        public override void OnLoseFocus()
        {
            UI_Manager.ui_Manager.DisableSecondaryInteractText();
        }

        public override void OnInteract()
        {

        }
    }
}


