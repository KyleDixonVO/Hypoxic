using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class RepairObject : MonoBehaviour
    {
        [SerializeField] Vector3 repairDestination;
        [SerializeField] float repairTime = 5.0f;
        [SerializeField] float elapsedRepairTime;
        [SerializeField] float repairDistance = 2.0f;
        [SerializeField] bool repaired = false;
        [SerializeField] string targetName;
        [SerializeField] Objective_Manager.Objectives objective;
        public bool repairing = false;
        public float repairPercentage;
        public RepairTarget targetObject;

        // Start is called before the first frame update
        void Start()
        {
            repairDestination = targetObject.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            FindRepairTarget();
            ToggleRepairText();
            CheckRepairStatus();
            Repair();
        }

        void Repair()
        {
            if (repaired) return;
            repairPercentage = (elapsedRepairTime / repairTime) * 100;
            if (WithinRepairRange() && InputManager.inputManager.rPressed && this.GetComponent<HeavyObject>().isHeld)
            {
                repairing = true;
                elapsedRepairTime += Time.deltaTime;
                Debug.Log(elapsedRepairTime / repairTime);
                // Play repairing audio
                AudioManager.audioManager.PlaySound(this.gameObject.GetComponent<AudioSource>(), AudioManager.audioManager.repairing);
            }
            else
            {
                repairing = false;
                elapsedRepairTime = 0;
            }

            if (elapsedRepairTime < repairTime) return;
            // Play sound Once when complete
            else if (!repaired)
            {
                AudioManager.audioManager.StopSound(this.gameObject.GetComponent<AudioSource>());
                AudioManager.audioManager.PlaySound(this.gameObject.GetComponent<AudioSource>(), AudioManager.audioManager.repairingComplete);
            }
            repaired = true;
            
        }

        void CheckRepairStatus() 
        { 
            if (!repaired || targetObject == null) return;
            targetObject.RepairedObject();
            this.GetComponent<HeavyObject>().ForceDropObject();
            this.gameObject.SetActive(false);
            Objective_Manager.objective_Manager.UpdateObjectiveCompletion((int)objective);
        }

        public bool WithinRepairRange()
        {
            if (Vector3.Distance(this.gameObject.transform.position, repairDestination) < repairDistance) return true;
            return false;
        }

        public void FindRepairTarget()
        {
            if (targetObject != null) return;
            try
            {
                targetObject = GameObject.Find(targetName).GetComponent<RepairTarget>();
            }
            catch
            {
                return;
            }
            
        }

        public void ToggleRepairText()
        {
            if (WithinRepairRange() && this.GetComponent<HeavyObject>().isHeld)
            {
                UI_Manager.ui_Manager.ActivateSecondaryInteractText();
            }
            else if (!WithinRepairRange() && this.GetComponent<HeavyObject>().isHeld)
            {
                UI_Manager.ui_Manager.DisableSecondaryInteractText();
            }
        }

    }

}