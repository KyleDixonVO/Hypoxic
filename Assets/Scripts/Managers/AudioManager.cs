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
        public AudioClip insideAmbiance;

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

        [Header("MusicSources")]
        public AudioSource musicAudio;
        [Header("SoundSources")]
        public AudioSource enviromentAudio;
        [Header("PlayerSources")]
        public AudioSource playerVoice;
        public AudioSource playerSuit;
        public AudioSource suitThruster;
        public AudioSource playerFootsteps;
        [Header("DoorSources")]

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
            // Referances
            FindPlayerSoundRefs();

            // Play
            PlayMusic();
            PlayRandomSound();

            // Stop
            StopSoundsIndoors();
            ManageSoundRandomness();

            // Player sounds
            CheckPlayerVoiceSounds();
            CheckPlayerSuitSounds();

            // Pause
            ManagePausedSound();
            
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
            if (!enviromentAudio.isPlaying)
            {
                // Starts numOfSounds at 0 in first loop
                int numOfSounds = -1;
                foreach (AudioClip clip in randomAtmosphereSounds)
                {
                    numOfSounds++;
                }
                int randomSoundSelected = Random.Range(0, numOfSounds);
                enviromentAudio.clip = randomAtmosphereSounds[randomSoundSelected];
            }
        }

        void ManageRandomPosition()
        {
            Vector3 audioHolderPos = randomSoundsObj.transform.position;
            Vector3 playerPos = PlayerStats.playerStats.gameObject.transform.position;

            audioHolderPos.x = playerPos.x + Random.Range(minSoundRange + 1, maxSoundRange);
            audioHolderPos.y = playerPos.y + Random.Range(minSoundRange + 1, maxSoundRange);
            audioHolderPos.z = playerPos.z + Random.Range(minSoundRange + 1, maxSoundRange);

            // Generates the max distance the sound can play.
            enviromentAudio.maxDistance = maxSoundRange * 2;
            randomSoundsObj.transform.position = audioHolderPos;
        }

        void PlayRandomSound()
        {
            //if (FirstPersonController_Sam.fpsSam == null) return;
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay || !FirstPersonController_Sam.fpsSam.inWater) return;
            //When sounds are finished playing and probability is 0. Choose a new spot for the sound to play.

            if (probability == 0 && enviromentAudio.isPlaying == false) 
            {
                ManageRandomPosition();
                Debug.LogWarning("RandomSoundPlaying");
                enviromentAudio.PlayOneShot(enviromentAudio.clip, (sfxVolume * masterVolume));
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

        void CheckPlayerVoiceSounds()
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

        void CheckPlayerSuitSounds()
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

        void CheckDoorSounds()
        {

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
                PauseSound(musicAudio);
                soundPaused = true;
            }
            else if (soundPaused)
            {
                ResumeSound(playerVoice);
                ResumeSound(playerSuit);
                ResumeSound(suitThruster);
                ResumeSound(playerFootsteps);
                ResumeSound(musicAudio);
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
            if (enviromentAudio) enviromentAudio.Pause();

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
            // Tobias Updated
            if (FirstPersonController_Sam.fpsSam == null) return;
            if (musicAudio == null) return;
            if (gameplayAmbiance == null) return;

            if (GameManager.gameManager.gameState == GameManager.gameStates.gameplay && FirstPersonController_Sam.fpsSam.inWater) musicAudio.clip = gameplayAmbiance;
            else if (GameManager.gameManager.gameState == GameManager.gameStates.gameplay && FirstPersonController_Sam.fpsSam.inWater == false) musicAudio.clip = insideAmbiance;
            else if (GameManager.gameManager.gameState == GameManager.gameStates.menu) musicAudio.clip = titleMusic;

            if (musicAudio.isPlaying == true) return;
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

            enviromentAudio.Stop();
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
