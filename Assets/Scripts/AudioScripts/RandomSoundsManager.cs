using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    // Code by Tobias
    public class RandomSoundsManager : MonoBehaviour
    {
        [Header("GameObjects")]
        public GameObject randomSoundsObj;

        [Header("Spherecast")]
        public Transform terrainCheck;
        public float terrainCheckRadius = 2;
        public LayerMask terrainLayerMask;

        [Header("Settings")]
        [Range(5f, 20f)]
        public float maxSoundRange = 10f;
        [Range(-5f, -20f)]
        public float minSoundRange = -10f;
        [Range(1f, 100000f)]
        public int probabilityWeight;
        public int probability;

        public bool playAudio = false;
        public bool touchingWall = false;

        public AudioClip selectedAtmosphereSound;      

        void Start()
        {
            randomSoundsObj.GetComponent<SphereCollider>();
        }

        // Update is called once per frame
        void Update()
        {
            PlayRandomSound();
            ManageSoundRandomness();
        }

        private void FixedUpdate()
        {
            probability = Random.Range(0, probabilityWeight);
        }

        void ManageSoundRandomness()
        {
            if (AudioManager.audioManager.AtmosphereAudio.isPlaying != true)
            {
                // Starts numOfSounds at 0 in first loop
                int numOfSounds = -1;
                foreach (AudioClip clip in AudioManager.audioManager.randomAtmosphereSounds)
                {
                    numOfSounds++;
                }
                int randomSoundSelected = Random.Range(0, numOfSounds);
                selectedAtmosphereSound = AudioManager.audioManager.randomAtmosphereSounds[randomSoundSelected];
            }
        }

        void MangeRandomPosition()
        {
            Vector3 audioHolderPos = randomSoundsObj.transform.position;
            Vector3 playerPos = PlayerStats.playerStats.gameObject.transform.position;

            audioHolderPos.x = playerPos.x + Random.Range(minSoundRange + 1, maxSoundRange);
            audioHolderPos.y = playerPos.y + Random.Range(minSoundRange + 1, maxSoundRange);
            audioHolderPos.z = playerPos.z + Random.Range(minSoundRange + 1, maxSoundRange);

            // Generates the max distance the sound can play.
            AudioManager.audioManager.AtmosphereAudio.maxDistance = playerPos.x + maxSoundRange * 2;
            randomSoundsObj.transform.position = audioHolderPos;
        }

        void PlayRandomSound()
        {
            if (FirstPersonController_Sam.fpsSam == null) return;
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay || !FirstPersonController_Sam.fpsSam.inWater) return;
            //When sounds are finished playing and probability is 0. Choose a new spot for the sound to play.
            if (AudioManager.audioManager.AtmosphereAudio.isPlaying != true)
            {
                if (probability == 0)
                {
                    MangeRandomPosition();
                    Debug.LogWarning("RandomSoundPlaying");
                    AudioManager.audioManager.AtmosphereAudio.clip = selectedAtmosphereSound;
                    AudioManager.audioManager.AtmosphereAudio.PlayOneShot(selectedAtmosphereSound, (AudioManager.audioManager.sfxVolume * AudioManager.audioManager.masterVolume));
                }

                if (IsTouchingTerrain() == true)
                {
                    //audioHolder.Stop();
                    //playAudio = false;
                }
            }
        }

        bool IsTouchingTerrain()
        {
            if (Physics.CheckSphere(terrainCheck.position, terrainCheckRadius, terrainLayerMask)) return true;
            return false;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, terrainCheckRadius);
        }
    }
}
