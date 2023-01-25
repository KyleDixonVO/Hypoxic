using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class AirlockButton : MonoBehaviour
    {
        public GameObject Door;
        [SerializeField]
        Airlock AL;

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player" && Input.GetKey(KeyCode.E) && !AL.isOpening)
            {
                AudioManager.audioManager.playSound(this.gameObject.GetComponent<AudioSource>());
                Debug.Log("Button pressed");
                AL.OpenDoor();
            }

        }
    }

}
