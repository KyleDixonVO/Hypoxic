using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class AirlockPlayerCheck : MonoBehaviour
    {
        public Airlock AL;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") AL.playerPresent = true;

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player") AL.playerPresent = false;
        }
    }

}
