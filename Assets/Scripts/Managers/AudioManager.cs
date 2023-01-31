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

        [Header("PlayerVoice")]
        public AudioClip heavyBreathing;

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
            
        }

        // Update is called once per frame
        void Update()
        {
            PlayMusic();
            StopSoundsIndoors();
        }


        public void playSound(AudioSource source, AudioClip clip)
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
            // stop a sound through the code's gameObject's audioManager
            source.Stop();
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

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
