using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    // Tobias :)
    public class PlayerSoundsManager : MonoBehaviour
    {
        public PlayerStats playerStats;

        public AudioSource playerVoice;
        public AudioSource playerSuit;
        public AudioSource playerFootsteps;

        private bool powerBelowHalf = false;
        private bool powerEmpty = false;

        public float halfSuitPower;

        // Start is called before the first frame update
        void Start()
        {
            halfSuitPower = playerStats.maxSuitPower / 2;
        }

        // Update is called once per frame
        void Update()
        {
            ManagePlayerVoice();
            ManagePlayerSuit();
            ManagePlayerFootsteps();
            ManagePausedSound();
            
        }

        void ManagePlayerVoice()
        {
            playerSuffocating();
        }

        void playerSuffocating()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;

            if (playerStats.suitPower <= 0)
            {
                AudioManager.audioManager.PlaySound(playerVoice.GetComponent<AudioSource>(), AudioManager.audioManager.heavyBreathing);
            }
            else
            {
                AudioManager.audioManager.StopSound(playerVoice.GetComponent<AudioSource>());
            }
        }

        void ManagePlayerSuit()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;

            if (playerStats.suitPower <= halfSuitPower && powerBelowHalf == false)
            {
                AudioManager.audioManager.StopSound(playerSuit.GetComponent<AudioSource>());
                AudioManager.audioManager.PlaySound(playerSuit.GetComponent<AudioSource>(), AudioManager.audioManager.lowPower);
                powerBelowHalf = true;
            }

            else if (playerStats.suitPower > halfSuitPower)
            {
                powerBelowHalf = false;
            }

            if (playerStats.suitPower <= 0 && powerEmpty == false)
            {
                AudioManager.audioManager.StopSound(playerSuit.GetComponent<AudioSource>());
                AudioManager.audioManager.PlaySound(playerSuit.GetComponent<AudioSource>(), AudioManager.audioManager.noPower);
                powerEmpty = true;
            }

            else if (playerStats.suitPower > 0)
            {
                powerEmpty = false;
            }

        }

        void ManagePlayerFootsteps()
        {

        }

        void ManagePausedSound()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay)
            {
                if (playerVoice.GetComponent<AudioSource>() != null)
                {
                    AudioManager.audioManager.PauseSound(playerVoice.GetComponent<AudioSource>());
                }
                if (playerSuit.GetComponent<AudioSource>() != null)
                {
                    AudioManager.audioManager.PauseSound(playerSuit.GetComponent<AudioSource>());
                }
                if (playerFootsteps.GetComponent<AudioSource>() != null)
                {
                    AudioManager.audioManager.PauseSound(playerFootsteps.GetComponent<AudioSource>());
                }
                AudioManager.audioManager.soundPaused = true;
            }
            else if (AudioManager.audioManager.soundPaused)
            {
                if (playerVoice.GetComponent<AudioSource>() != null)
                {
                    AudioManager.audioManager.ResumeSound(playerVoice.GetComponent<AudioSource>());
                }
                if (playerSuit.GetComponent<AudioSource>() != null)
                {
                    AudioManager.audioManager.ResumeSound(playerSuit.GetComponent<AudioSource>());
                }
                if (playerFootsteps.GetComponent<AudioSource>() != null)
                {
                    AudioManager.audioManager.ResumeSound(playerFootsteps.GetComponent<AudioSource>());
                }
                AudioManager.audioManager.soundPaused = false;
            }
            else if (!playerSuit.GetComponent<AudioSource>().isPlaying && GameManager.gameManager.gameState != GameManager.gameStates.paused)
            {
                AudioManager.audioManager.StopSound(playerSuit.GetComponent<AudioSource>());
            }
        }
    }
}
