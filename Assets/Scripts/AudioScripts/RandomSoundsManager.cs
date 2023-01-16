using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundsManager : MonoBehaviour
{
    public static RandomSoundsManager RSM;

    [Header("SoundPrefs")]
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;

    [Header("AuidoClips")]
    public AudioClip soundOne;
    public AudioClip titleMusic;
    public AudioClip gameplayAmbiance;

    [Header("GameObjects")]
    public GameObject playerObj;
    public GameObject randomSoundsObj;

    [Header("Components")]
    public AudioSource randomSoundsAudio;
    public AudioSource musicAudio;

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

    private void Awake()
    {
        if (RSM == null)
        {
            RSM = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (RSM != null && RSM != this)
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        randomSoundsObj.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        FindObjRefs();
        PlayMusic();
        PlayRandomSound();
        StopSoundsIndoors();
    }

    private void FixedUpdate()
    {
        probability = Random.Range(0, probabilityWeight);
    }

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
        if (randomSoundsAudio) randomSoundsAudio.Pause();

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

    void PlayRandomSound()
    {
        if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay || !FirstPersonController_Sam.fpsSam.inWater) return;
        // When sounds are finished playing and probability is 0. Choose a new spot for the sound to play.
        if (randomSoundsAudio.isPlaying != true)
        {
            if (probability == 0)
            {
                MangeRandomPosition();
                randomSoundsAudio.clip = soundOne;
                randomSoundsAudio.PlayOneShot(soundOne, (sfxVolume * masterVolume));
            }

            if (IsTouchingTerrain() == true)
            {
                //audioHolder.Stop();
                //playAudio = false;
            }
        }
    }

    void MangeRandomPosition()
    {
        Vector3 audioHolderPos = randomSoundsObj.transform.position;
        Vector3 playerPos = playerObj.transform.position;

        audioHolderPos.x = playerPos.x + Random.Range(minSoundRange + 1, maxSoundRange);
        audioHolderPos.y = playerPos.y + Random.Range(minSoundRange + 1, maxSoundRange);
        audioHolderPos.z = playerPos.z + Random.Range(minSoundRange + 1, maxSoundRange);

        // Generates the max distance the sound can play.
        randomSoundsAudio.maxDistance = playerPos.x + maxSoundRange * 2;
        randomSoundsObj.transform.position = audioHolderPos;
    }

    bool IsTouchingTerrain()
    {
        if (Physics.CheckSphere(terrainCheck.position, terrainCheckRadius, terrainLayerMask)) return true;
        return false;
    }

    void FindObjRefs()
    {
        if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
        if (playerObj != null && randomSoundsObj != null) return;
        playerObj = GameObject.Find("Player");
        randomSoundsObj = GameObject.Find("RandomSoundsHolder");
    }

    void StopSoundsIndoors()
    {
        if (FirstPersonController_Sam.fpsSam != null)
        {
            if (FirstPersonController_Sam.fpsSam.inWater) return;
        }
        
        randomSoundsAudio.Stop();
        musicAudio.Stop();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, terrainCheckRadius);
    }
}
