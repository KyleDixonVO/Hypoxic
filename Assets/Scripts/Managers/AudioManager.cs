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
        public AudioClip uIButtonPassover;
        public AudioClip sonarPing;

        [Header("DoorAudioClips")]
        public AudioClip doorOpening;
        public AudioClip doorOpened;
        public AudioClip doorClosing;
        public AudioClip doorClosed;
        public AudioClip doorButton;


        [Header("AtmosphereAudioClips")]
        public List<AudioClip> randomEnviromentSounds = new List<AudioClip>();


        [Header("MusicAudioClips")]
        public AudioClip titleMusic;
        public AudioClip outsideAmbiance;
        public AudioClip insideAmbiance;


        [Header("SuitClips")]
        public AudioClip jets;


        [Header("PlayerVoiceClips")]
        public AudioClip heavyBreathing;


        [Header("AIVoiceClips")]
        public AudioClip lowPower;
        public AudioClip noPower;


        [Header("BigMonsterAudioClips")]
        public AudioClip bigFishBite;
        public AudioClip bigFishAgro;
        public AudioClip bigFishHurt;
        public AudioClip bigFishFleeing;

        [Header("SmallMonsterAudioClips")]
        public AudioClip smallFishBite;
        public AudioClip smallFishAgro;
        public AudioClip smallFishHurt;
        public AudioClip smallFishDying;
        public AudioClip smallFishScream;

        [Header("SmallMonsterAudioClips")]
        public AudioClip smallBite;

        [Header("WeaponAudioClips")]
        public AudioClip electricProdShock;
        public AudioClip electricProdRecharge;
        public AudioClip electricProdNoCharge;
        public AudioClip harpoonShot;
        public AudioClip harpoonReloading;
        public AudioClip harpoonNoAmmo;

        [Header("Tools")]
        public AudioClip batteryUsed;
        public AudioClip glowstickUsed;
        public AudioClip syringeUsed;

        [Header("ObjectiveAudioClips")]
        public AudioClip repairing;
        public AudioClip repairingComplete;
        public AudioClip pickupPipe;
        public AudioClip dropPipe;

        [Header("Inventory")]
        public AudioClip swapItem;
        public AudioClip pickupItem;

        [Header("Audio Logs")]
        public AudioClip[] audioLogs;

        // -----------------AudioSources----------------------
        [Header("AudioSources")]
        public AudioSource musicAudio;
        public AudioSource UiAudio;
        public AudioSource enviromentAudio;


        // Player Sounds Manager --------------------------------------------------------------------------
        public AudioSource playerVoiceAudio;
        public AudioSource playerSuitAudio;
        public AudioSource suitThrusterAudio;
        public AudioSource playerInventoryAudio;

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
        [Range(1f, 10000f)]
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
            StopSoundsIndoors();

            // Player sounds
            CheckPlayerVoiceSounds();
            CheckPlayerSuitSounds();
            CheckPlayerInventorySounds();

            // Pause
            ManagePausedSound();
        }


        private void FixedUpdate()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
            probability = Random.Range(0, probabilityWeight);
        }


        public void PlaySound(AudioSource source, AudioClip clip)
        {
            // Play a sound through the code's gameObject's audioManager
            if (source.isPlaying) return;
            source.clip = clip;
            source.volume = (sfxVolume * masterVolume);
            source.Play();
            //Debug.LogWarning("SoundPlayed");
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
            source.UnPause();
        }

        // Audio Randomizer -------------------------------------------------------------------------------------------------------------
        void ManageSoundRandomness()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay || FirstPersonController_Sam.fpsSam.inWater == false) return;
            if (enviromentAudio.isPlaying) return;

            // Starts numOfSounds at 0 in first loop
            int numOfSounds = 0;
            foreach (AudioClip clip in randomEnviromentSounds)
            {
                numOfSounds++;
            }
            int randomSoundSelected = Random.Range(0, numOfSounds);
            enviromentAudio.clip = randomEnviromentSounds[randomSoundSelected];
            //Debug.LogWarning(randomSoundSelected);

        }


        void MangaeRandomPosition()
        {
            Vector3 audioHolderPos = randomSoundsObj.transform.position;
            Vector3 playerPos = PlayerStats.playerStats.gameObject.transform.position;


            audioHolderPos.x = playerPos.x + Random.Range(minSoundRange + 1, maxSoundRange);
            audioHolderPos.y = playerPos.y + Random.Range(minSoundRange + 1, maxSoundRange);
            audioHolderPos.z = playerPos.z + Random.Range(minSoundRange + 1, maxSoundRange);


            // Generates the max distance the sound can play.
            enviromentAudio.maxDistance = playerPos.x + maxSoundRange * 2;
            randomSoundsObj.transform.position = audioHolderPos;
        }


        void PlayRandomSound()
        {
            if (FirstPersonController_Sam.fpsSam == null) return;
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay || !FirstPersonController_Sam.fpsSam.inWater) return;
            //When sounds are finished playing and probability is 0. Choose a new spot for the sound to play.
            if (enviromentAudio.isPlaying == true) return;

            ManageSoundRandomness();

            if (probability <= 1)
            {
                MangaeRandomPosition();
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
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay || playerVoiceAudio == null) return;


            if (PlayerStats.playerStats.suitPower > 0)
            {
                StopSound(playerVoiceAudio);
                return;
            }
            PlaySound(playerVoiceAudio, heavyBreathing);
        }


        void CheckPlayerSuitSounds()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
            // Player must be in water for suit to function
            if (FirstPersonController_Sam.fpsSam.inWater == false)
            {
                StopSound(suitThrusterAudio);
                StopSound(playerSuitAudio);
                return;
            }

            // Plays low power sound
            if (PlayerStats.playerStats.suitPower <= PlayerStats.playerStats.maxSuitPower / 2 && playedPowerBelowHalf == false)
            {
                StopSound(playerSuitAudio);
                PlaySound(playerSuitAudio, lowPower);
                playedPowerBelowHalf = true;
            }
            else if (PlayerStats.playerStats.suitPower > PlayerStats.playerStats.maxSuitPower / 2) playedPowerBelowHalf = false;


            // Plays no power sound
            if (PlayerStats.playerStats.suitPower <= 0 && playedPowerEmpty == false)
            {
                StopSound(playerSuitAudio);
                PlaySound(playerSuitAudio, noPower);
                playedPowerEmpty = true;
            }
            else if (PlayerStats.playerStats.suitPower > 0) playedPowerEmpty = false;


            // Plays running sound
            if (FirstPersonController_Sam.fpsSam.IsRunning())
            {
                if (!suitThrusterAudio.isPlaying) PlaySound(suitThrusterAudio, jets);
            }

            if (FirstPersonController_Sam.fpsSam.IsRunning() == false) StopSound(suitThrusterAudio);


            // Stop running sound when holding a pipe
            if (FirstPersonController_Sam.fpsSam.carryingHeavyObj) StopSound(suitThrusterAudio);
        }

        void CheckPlayerInventorySounds()
        {
            // Play inventroy swap sound
            if (Input.GetKeyDown(KeyCode.Alpha1) && PlayerInventory.playerInventory.inventory[0] != null
                || Input.GetKeyDown(KeyCode.Alpha2) && PlayerInventory.playerInventory.inventory[1] != null
                || Input.GetKeyDown(KeyCode.Alpha3) && PlayerInventory.playerInventory.inventory[2] != null)
            {
                StopSound(playerInventoryAudio);
                PlaySound(playerInventoryAudio, swapItem);
            }
        }


        void ManagePausedSound()
        {
            // Pauses sounds when game is paused
            if (playerSuitAudio == null || playerVoiceAudio == null || playerInventoryAudio == null) return;
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay)
            {
                PauseSound(playerVoiceAudio);
                PauseSound(playerSuitAudio);
                PauseSound(suitThrusterAudio);
                PauseSound(playerInventoryAudio);               
                PauseSound(enviromentAudio);

                // Stops menu music from being paused
                if (GameManager.gameManager.gameState != GameManager.gameStates.menu)
                {
                    PauseSound(musicAudio);
                }
                soundPaused = true;
            }
            else if (soundPaused)
            {
                ResumeSound(playerVoiceAudio);
                ResumeSound(playerSuitAudio);
                ResumeSound(suitThrusterAudio);
                ResumeSound(playerInventoryAudio);
                ResumeSound(musicAudio);
                ResumeSound(enviromentAudio);
                soundPaused = false;
            }

            if (!playerSuitAudio.isPlaying && GameManager.gameManager.gameState != GameManager.gameStates.paused)
            {
                StopSound(playerSuitAudio.GetComponent<AudioSource>());
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
            //Debug.Log("Master: " + masterVolume + "Music: " + musicVolume + "SFX: " + sfxVolume);
            SaveVolumePrefs();
        }

        public void SaveVolumePrefs()
        {
            Debug.Log("Saving Volume Prefs");
            if (Data_Manager.dataManager.mastervolume == masterVolume && Data_Manager.dataManager.musicVolume == musicVolume && Data_Manager.dataManager.SFXVolume == sfxVolume) return;
            Data_Manager.dataManager.musicVolume = musicVolume;
            Data_Manager.dataManager.mastervolume = masterVolume;
            Data_Manager.dataManager.SFXVolume = sfxVolume;
        }

        void PlayMusic()
        {
            if (musicAudio == null) return;

            // Onside music clip
            if (GameManager.gameManager.gameState == GameManager.gameStates.gameplay && musicAudio != null && outsideAmbiance != null && FirstPersonController_Sam.fpsSam.inWater) musicAudio.clip = outsideAmbiance;
            // Inside music clip
            else if (GameManager.gameManager.gameState == GameManager.gameStates.gameplay && musicAudio != null && insideAmbiance != null && FirstPersonController_Sam.fpsSam.inWater == false) musicAudio.clip = insideAmbiance;
            // Title music clip
            else if (GameManager.gameManager.gameState == GameManager.gameStates.menu && musicAudio != null && titleMusic) musicAudio.clip = titleMusic;
            
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
            if (playerInventoryAudio != null && playerVoiceAudio != null && playerSuitAudio != null) return;
            playerSuitAudio = GameObject.Find("playerSuitSound").GetComponent<AudioSource>();
            playerVoiceAudio = GameObject.Find("playerVoiceSound").GetComponent<AudioSource>();
            suitThrusterAudio = GameObject.Find("suitThrusterSound").GetComponent<AudioSource>();
            playerInventoryAudio = GameObject.Find("playerInventorySound").GetComponent<AudioSource>();
        }


        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
}

