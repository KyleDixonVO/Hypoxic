using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class IntroAudioTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                AudioManager.audioManager.playIntroAudio = true;
                Debug.Log("Intro");
                Destroy(gameObject);
            }
            
        }
    }
}
