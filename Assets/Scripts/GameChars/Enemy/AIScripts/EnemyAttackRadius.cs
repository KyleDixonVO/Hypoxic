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
            if (other.tag != "Player") return;
            playerInRadius = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag != "Player") return;
            playerInRadius = false;
        }
    }
}
