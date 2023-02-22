using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class AirlockNoLoadTrigger : MonoBehaviour
    {
        public Airlock exteriorAirlock;
        public Airlock interiorAirlock;

        private void OnTriggerEnter(Collider other) 
        {
            if (other.tag == "Player") 
            {
                //Debug.Log("player present");
                if (exteriorAirlock.isOpening)
                {
                    exteriorAirlock.CloseDoor();
                    interiorAirlock.timerActive = true;
                    interiorAirlock.isOpening = true;
                }
                else if (interiorAirlock.isOpening)
                {
                    interiorAirlock.CloseDoor();
                    exteriorAirlock.timerActive = true;
                    exteriorAirlock.isOpening = true;
    
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                exteriorAirlock.CloseDoor();
                interiorAirlock.CloseDoor();
            }

        }
    }
}
