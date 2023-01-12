using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundsManager : MonoBehaviour
{
    public static RandomSoundsManager RSM;

    [Header("AuidoClips")]
    public AudioClip soundOne;

    [Header("GameObjects")]
    public GameObject playerObj;
    public GameObject randomSoundsObj;

    [Header("Components")]
    public AudioSource randomSoundsAudio;

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
        // When sounds are finished playing and probability is 0. Choose a new spot for the sound to play.
        FindObjRefs();
        if (randomSoundsAudio.isPlaying != true)
        {
            if (probability == 0)
            {
                MangeRandomPosition();
                randomSoundsAudio.clip = soundOne;
                randomSoundsAudio.PlayOneShot(soundOne);
            }

            if (IsTouchingTerrain() == true)
            {
                //audioHolder.Stop();
                //playAudio = false;
            }
        }
    }

    private void FixedUpdate()
    {
        probability = Random.Range(0, probabilityWeight);
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
        if (playerObj != null && randomSoundsObj != null) return;
        playerObj = GameObject.Find("Player");
        randomSoundsObj = GameObject.Find("RandomSoundsHolder");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, terrainCheckRadius);
    }
}
