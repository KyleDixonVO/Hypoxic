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
            if (other.tag == "Player")
            {
                AL.playerPresent = true;
                AL.CloseDoor();
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player") AL.playerPresent = false;

            if (!Level_Manager.LM.IsSceneOpen("DemoBuildingInside"))
            {
                if (other.tag == "Player")
                {
                    AL.CloseDoor();
                }
            }
        }
    }
}
