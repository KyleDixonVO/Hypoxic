using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror 
{
    public class HideSFX : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<FirstPersonController_Sam>() == null) return;
            else if (other.gameObject.GetComponent<FirstPersonController_Sam>()) UI_Manager.ui_Manager.VignetteEffectOn(false);         
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<FirstPersonController_Sam>() == null) return;
            else if (other.gameObject.GetComponent<FirstPersonController_Sam>()) UI_Manager.ui_Manager.VignetteEffectOff();
        }

    }
}


