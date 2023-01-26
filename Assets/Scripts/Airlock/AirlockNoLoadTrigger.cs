using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class AirlockNoLoadTrigger : MonoBehaviour
    {
        public Airlock airlockToOpen;
        public Airlock airlockToClose;

        private void OnTriggerEnter(Collider other)
        {           
            airlockToOpen.OpenDoor();
            airlockToClose.CloseDoor(0f);
        }
    }
}
