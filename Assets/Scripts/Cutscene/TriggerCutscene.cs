using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror 
{    
    public class TriggerCutscene : MonoBehaviour
    {
        public Animator animator;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            if (other.transform.tag == "Player" && Objective_Manager.objective_Manager.CanCompleteFinalObjective())
            {
                animator.SetBool("MissionComplete", true);
            }
        }
    }
}

