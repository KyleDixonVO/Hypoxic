using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    // Code by Tobias
    public class EnemyAttackRadius : MonoBehaviour
    {
        public bool playerInRadius = false;
        private void OnTriggerStay(Collider other)
        {
            playerInRadius = true;
        }

        private void OnTriggerExit(Collider other)
        {
            playerInRadius = false;
        }
    }
}
