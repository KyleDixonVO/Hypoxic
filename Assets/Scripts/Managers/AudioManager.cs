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

        [Header("DoorAudioClips")]
        public AudioClip doorOpening;
        public AudioClip doorOpened;
        public AudioClip doorClosing;
        public AudioClip doorClosed;

        [Header("AtmosphereAudioClips")]
        public List<AudioClip> randomAtmosphereSounds = new List<AudioClip>();

        [Header("MusicAudioClips")]
        public AudioClip titleMusic;
        public AudioClip gameplayAmbiance;

        [Header("BigMonsterAudioClips")]
        public AudioClip bigBite;
        public AudioClip bigAgro;

        [Header("SmallMonsterAudioClips")]
        public AudioClip smallBite;

        [Header("AudioSources")]
        public AudioSource musicAudio;
        public AudioSource BigMonsterAudio;
        public AudioSource smallMonsterAudio;
        public AudioSource AtmosphereAudio;
        public AudioSource DoorAudio;

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

        void StopMusic()
        {
            musicAudio.Stop();
        }

        public void playSound(AudioSource source)
        {
            source.volume = (sfxVolume * masterVolume);
            source.PlayOneShot(source.clip);
            Debug.LogWarning("SoundPlayed");
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
            if (musicAudio.isPlaying == true || UI_Manager.ui_Manager.OptionsOpen()) return;

            if (GameManager.gameManager.gameState == GameManager.gameStates.gameplay && musicAudio != null && gameplayAmbiance != null) musicAudio.clip = gameplayAmbiance;
            else if (GameManager.gameManager.gameState == GameManager.gameStates.menu && musicAudio != null && gameplayAmbiance != null) musicAudio.clip = titleMusic;

            musicAudio.volume = (musicVolume * masterVolume);
            musicAudio.Play();
        }
        
        void StopSoundsIndoors()
        {
            if (FirstPersonController_Sam.fpsSam != null)
            {
                if (FirstPersonController_Sam.fpsSam.inWater) return;
            }

            AtmosphereAudio.Stop();
            musicAudio.Stop();
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

        // Made by Edmund
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
        public void PlaySoundBigAgro(AudioSource source)
        {
            source.PlayOneShot(bigAgro);
            Debug.LogWarning("big mad");
        }

        public void PlaySoundBigBite(AudioSource source)
        {
            source.PlayOneShot(bigBite);
            Debug.LogWarning("big bite");
        }

        public void PlaySoundSmallBite(AudioSource source)
        {
            source.PlayOneShot(smallBite);
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
