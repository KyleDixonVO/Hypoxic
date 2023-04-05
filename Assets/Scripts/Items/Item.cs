using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class Item : Interactable
    {
        [Header("Usage Timers")]
        [SerializeField]
        protected float usageTime;
        [SerializeField]
        protected float usageProgress;

        [Header("Refs")]
        [SerializeField]
        protected GameObject playerStats;
        [SerializeField]
        protected PlayerStats PS;

        [SerializeField] string tooltip;

        [Header("bools")]
        [SerializeField]
        public bool isEquiped;
        [SerializeField]
        protected bool beingUsed;
        public bool isUsed = false;

        [Header("sprite")]
        public Sprite icon;

        // Tobias was here
        protected AudioSource itemAudioSource;
        protected Outline _outline;

        private void Start()
        {

        }

        public override void ResetForNewRun()
        {
            base.ResetForNewRun();
            isUsed = false;
            isEquiped = false;
            transform.parent = Interactable_Manager.interactable_manager.gameObject.transform;
            beingUsed = false;
            usageProgress = usageTime;
        }

        protected void TimeToEffect()
        {      
            if (usageProgress <= 0) // Apply effect
            {
                usageProgress = 0;
                // activets ApplyEffect()
                ApplyEffect();
                beingUsed = false;
                isUsed = true;
            }
            else if (usageProgress > 0 && isEquiped) // Timer
            {
                usageProgress -= Time.deltaTime;
            }
            else if (!isEquiped) // stop timer if unequiped
            {
                beingUsed = false;
                usageProgress = usageTime;
            }
        }

        virtual protected void ApplyEffect()
        {

        }
        protected void Unequip()
        {
            isEquiped = false;
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        protected void Equip()
        {
            isEquiped = true;
            gameObject.GetComponent<Renderer>().enabled = true;
        }

        // ----------------------------------- Interactable ---------------------- \\
        public override void OnInteract()
        {
            PlayerInventory.playerInventory.AddToInventory(this.gameObject);
            _outline.enabled = false;
            //throw new System.NotImplementedException();
        }

        public override void OnFocus()
        {
            string tempTooltip;
            if (PlayerInventory.playerInventory.inventoryFull) tempTooltip = PlayerInventory.playerInventory.fullInventoryTooltip;
            else tempTooltip = tooltip;

            UI_Manager.ui_Manager.ActivatePrimaryInteractText(tempTooltip);

            if (this.gameObject.GetComponent<Outline>() != null && transform.parent != FirstPersonController_Sam.fpsSam.playerCamera.gameObject)
            {
                Debug.Log("Setting active outline");
                gameObject.GetComponent<Outline>().SetAsActiveOutline();
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
