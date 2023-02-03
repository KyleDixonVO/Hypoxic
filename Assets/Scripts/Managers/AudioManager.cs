using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    // Managed by Tobias
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager audioManager;

        [Header("SoundPrefs")]
        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;

        [Header("UIAudioClips")]
        public AudioClip uIButton;

        [Header("DoorAudioClips")]
        public AudioClip doorOpening;
        public AudioClip doorOpened;
        public AudioClip doorClosing;
        public AudioClip doorClosed;
        public AudioClip doorButton;

        [Header("AtmosphereAudioClips")]
        public List<AudioClip> randomAtmosphereSounds = new List<AudioClip>();

        [Header("MusicAudioClips")]
        public AudioClip titleMusic;
        public AudioClip gameplayAmbiance;

        [Header("SuitClips")]
        public AudioClip jets;

        [Header("PlayerVoiceClips")]
        public AudioClip heavyBreathing;

        [Header("AIVoiceClips")]
        public AudioClip lowPower;
        public AudioClip noPower;

        [Header("BigMonsterAudioClips")]
        public AudioClip bigBite;
        public AudioClip bigAgro;

        [Header("SmallMonsterAudioClips")]
        public AudioClip smallBite;

        [Header("WeaponAudioClips")]
        public AudioClip electricProdShock;
        public AudioClip electricProdRecharge;
        public AudioClip electricProdNoCharge;
        public AudioClip harpoonShot;
        public AudioClip harpoonReloading;
        public AudioClip harpoonNoAmmo;

        [Header("AudioSources")]
        public AudioSource musicAudio;
        public AudioSource AtmosphereAudio;

        // Pulled elsewhere
        public bool soundPaused = false;

        // Audio Randomizer --------------------------------------------------------------------------------
        [Header("Audio Randomizer")]
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

        // Player Sounds Manager --------------------------------------------------------------------------
        [Header("Player Sounds")]
        public AudioSource playerVoice;
        public AudioSource playerSuit;
        public AudioSource suitThruster;
        public AudioSource playerFootsteps;

        private bool playedPowerBelowHalf = false;
        private bool playedPowerEmpty = false;




        private void Awake()
        {
            if (audioManager == null)
            {
                audioManager = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else if (audioManager != null && audioManager != this)
            {
                Destroy(this.gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            randomSoundsObj.GetComponent<SphereCollider>();
        }

        // Update is called once per frame
        void Update()
        {
            PlayMusic();
            StopSoundsIndoors();
            PlayRandomSound();
            ManageSoundRandomness();
            ManagePlayerVoice();
            ManagePlayerSuit();
            ManagePausedSound();
            FindPlayerSoundRefs();
        }

        private void FixedUpdate()
        {
            probability = Random.Range(0, probabilityWeight);
        }

        public void PlaySound(AudioSource source, AudioClip clip)
        {
            // Play a sound through the code's gameObject's audioManager
            if (source.isPlaying) return;
            source.clip = clip;
            source.volume = (sfxVolume * masterVolume);
            source.Play();
            Debug.LogWarning("SoundPlayed");
        }

        public void StopSound(AudioSource source)
        {
            // Stop a sound through the code's gameObject's audioManager
            source.Stop();
            source.clip = null;
        }

        public void PauseSound(AudioSource source)
        {
            // Pause a sound through the code's gameObject's audioManager
            source.Pause();
        }

        public void ResumeSound(AudioSource source)
        {
            // Resume a sound through the code's gameObject's audioManager
            source.Play();
        }

        // Audio Randomizer -------------------------------------------------------------------------------------------------------------
        void ManageSoundRandomness()
        {
            if (AtmosphereAudio.isPlaying != true)
            {
                // Starts numOfSounds at 0 in first loop
                int numOfSounds = -1;
                foreach (AudioClip clip in randomAtmosphereSounds)
                {
                    numOfSounds++;
                }
                int randomSoundSelected = Random.Range(0, numOfSounds);
                AtmosphereAudio.clip = randomAtmosphereSounds[randomSoundSelected];
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
            AtmosphereAudio.maxDistance = playerPos.x + maxSoundRange * 2;
            randomSoundsObj.transform.position = audioHolderPos;
        }

        void PlayRandomSound()
        {
            if (FirstPersonController_Sam.fpsSam == null) return;
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay || !FirstPersonController_Sam.fpsSam.inWater) return;
            //When sounds are finished playing and probability is 0. Choose a new spot for the sound to play.
            if (AtmosphereAudio.isPlaying == true) return;

            if (probability == 0)
            {
                MangeRandomPosition();
                Debug.LogWarning("RandomSoundPlaying");
                AtmosphereAudio.PlayOneShot(AtmosphereAudio.clip, (sfxVolume * masterVolume));
            }

            if (IsTouchingTerrain() == true)
            {
                //audioHolder.Stop();
                //playAudio = false;
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

        // Player Sounds --------------------------------------------------------------------------------------------------------------------------------------------

        void ManagePlayerVoice()
        {
            playerSuffocating();
        }

        void playerSuffocating()
        {
            // Plays suffocating sound
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay || playerVoice == null) return;

            if (PlayerStats.playerStats.suitPower > 0)
            {
                StopSound(playerVoice);
                return;
            }
            PlaySound(playerVoice, heavyBreathing);
        }

        void ManagePlayerSuit()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay || playerVoice == null) return;

            // Plays low power sound
            if (PlayerStats.playerStats.suitPower <= PlayerStats.playerStats.maxSuitPower/2 && playedPowerBelowHalf == false)
            {
                StopSound(playerSuit);
                PlaySound(playerSuit, lowPower);
                playedPowerBelowHalf = true;
            }
            else if (PlayerStats.playerStats.suitPower > PlayerStats.playerStats.maxSuitPower/2) playedPowerBelowHalf = false;

            // Plays no power sound
            if (PlayerStats.playerStats.suitPower <= 0 && playedPowerEmpty == false)
            {
                StopSound(playerSuit);
                PlaySound(playerSuit, noPower);
                playedPowerEmpty = true;
            }
            else if (PlayerStats.playerStats.suitPower > 0) playedPowerEmpty = false;

            // Plays running sound
            if (FirstPersonController_Sam.fpsSam.IsRunning())
            {
                if (!suitThruster.isPlaying) PlaySound(suitThruster, jets);
            }
            else if (FirstPersonController_Sam.fpsSam.IsRunning() == false) StopSound(suitThruster); 
        }

        void ManagePausedSound()
        {
            // Pauses sounds when game is paused
            if (playerSuit == null || playerVoice == null || playerFootsteps == null) return;
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay)
            { 
                PauseSound(playerVoice);
                PauseSound(playerSuit);
                PauseSound(suitThruster);
                PauseSound(playerFootsteps);
                soundPaused = true;
            }
            else if (soundPaused)
            {
                ResumeSound(playerVoice);
                ResumeSound(playerSuit);
                ResumeSound(suitThruster);
                ResumeSound(playerFootsteps);
                soundPaused = false;
            }
            
            if (!playerSuit.isPlaying && GameManager.gameManager.gameState != GameManager.gameStates.paused)
            {
                StopSound(playerSuit.GetComponent<AudioSource>());
            }
        }



        // Made by Kyle 
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void LoadVolumePrefs()
        {
            masterVolume = Data_Manager.dataManager.mastervolume;
            musicVolume = Data_Manager.dataManager.musicVolume;
            sfxVolume = Data_Manager.dataManager.SFXVolume;
        }

        public void UpdateVolumePrefs()
        {
            if (!UI_Manager.ui_Manager.OptionsOpen()) return;
            if (musicAudio.isPlaying) musicAudio.Pause();
            if (AtmosphereAudio) AtmosphereAudio.Pause();

            masterVolume = UI_Manager.ui_Manager.sliderMaster.value;
            musicVolume = UI_Manager.ui_Manager.sliderMusic.value;
            sfxVolume = UI_Manager.ui_Manager.sliderSFX.value;
            Debug.Log("Master: " + masterVolume + "Music: " + musicVolume + "SFX: " + sfxVolume);
        }

        public void SaveVolumePrefs()
        {
            Debug.Log("Saving Volume Prefs");
            Data_Manager.dataManager.musicVolume = musicVolume;
            Data_Manager.dataManager.mastervolume = masterVolume;
            Data_Manager.dataManager.SFXVolume = sfxVolume;
        }
        void PlayMusic()
        {
            if (musicAudio == null) return;
            if (musicAudio.isPlaying == true) return;

            if (GameManager.gameManager.gameState == GameManager.gameStates.gameplay && musicAudio != null && gameplayAmbiance != null) musicAudio.clip = gameplayAmbiance;
            else if (GameManager.gameManager.gameState == GameManager.gameStates.menu && musicAudio != null && gameplayAmbiance != null) musicAudio.clip = titleMusic;

            musicAudio.volume = (musicVolume * masterVolume);
            musicAudio.Play();
        }
        
        void StopSoundsIndoors()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
            if (FirstPersonController_Sam.fpsSam != null)
            {
                if (FirstPersonController_Sam.fpsSam.inWater) return;
            }

            AtmosphereAudio.Stop();
            musicAudio.Stop();
        }

        void FindPlayerSoundRefs()
        {
            if (FirstPersonController_Sam.fpsSam == null) return;
            if (playerFootsteps != null && playerVoice != null && playerSuit != null) return;
            playerSuit = GameObject.Find("playerSuitSound").GetComponent<AudioSource>();
            playerVoice = GameObject.Find("playerVoiceSound").GetComponent<AudioSource>();
            suitThruster = GameObject.Find("suitThrusterSound").GetComponent<AudioSource>();
            playerFootsteps = GameObject.Find("Player").GetComponent<AudioSource>();
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
