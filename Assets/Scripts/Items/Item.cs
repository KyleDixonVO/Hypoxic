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

        [Header("bools")]
        [SerializeField]
        public bool isEquiped;
        [SerializeField]
        protected bool beingUsed;
        public bool isUsed = false;

        private void Start()
        {

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
            //throw new System.NotImplementedException();
        }

        public override void OnFocus()
        {
            UI_Manager.ui_Manager.ActivatePrimaryInteractText();
            //throw new System.NotImplementedException();
        }

        public override void OnLoseFocus()
        {
            UI_Manager.ui_Manager.DisablePrimaryInteractText();
           // throw new System.NotImplementedException();
        }
    }

}
