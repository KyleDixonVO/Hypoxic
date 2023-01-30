using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class PlayerVoice : MonoBehaviour
    {
        public PlayerStats playerStats;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (playerStats.suitPower <= 0 && GameManager.gameManager.gameState == GameManager.gameStates.gameplay)
            {
                AudioManager.audioManager.playSound(this.GetComponent<AudioSource>(), AudioManager.audioManager.heavyBreathing);
            }
            else
            {
                AudioManager.audioManager.StopSound(this.GetComponent<AudioSource>());
            }
        }
    }
}
