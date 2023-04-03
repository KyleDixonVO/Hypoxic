using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Rendering;

namespace UnderwaterHorror
{
    public class StartTrigger : MonoBehaviour
    {
        public GameObject lift;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag != "Player") return;
            other.transform.SetParent(lift.transform, true);
            FirstPersonController_Sam.fpsSam.DisableCharacterController();
            FirstPersonController_Sam.fpsSam.inWater = true;
            RenderSettings.fog = true;
        }
    }
}

