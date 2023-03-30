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
            if (Objective_Manager.objective_Manager.CanCompleteFinalObjective())
            {
                Debug.Log("here");
                animator.SetBool("MissionComplete", true);
            }
        }
    }
}

