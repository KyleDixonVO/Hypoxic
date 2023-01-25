using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnderwaterHorror;

public class Airlock : MonoBehaviour
{
    [Header("Refences")]
    public Level_Manager levelManager;
    [SerializeField] GameObject doorRight;
    [SerializeField] GameObject doorLeft;
    [SerializeField] GameObject otherDoor;
    Airlock otherAirlock;

    [Header("Door Positions")]
    [SerializeField] GameObject rightOpenPos;
    [SerializeField] GameObject leftOpenPos;
    Vector3 rightClosePos;
    Vector3 leftClosePos;

    [Header("Booleans")]
    public bool isOpening = false;
    [SerializeField] bool isOpenable = true;
    public bool playerPresent = false;
    [SerializeField] bool isLoad;
    bool canLoad;

    [Header("Values")]
    [SerializeField] float openWaitTime = 10f;
    [SerializeField] float closeWaitTime = 20f;
    float countDownProgress = 5f;

    // Start is called before the first frame update
    void Start()
    {
        // Set close pos
        rightClosePos = doorRight.transform.position;
        leftClosePos = doorLeft.transform.position;

        // set variables
        if (!isLoad) countDownProgress = 0;
        if (otherDoor != null) otherAirlock = otherDoor.GetComponent<Airlock>();
    }

    // --------------------------------------- Door Controls --------------------------------------- \\
    // Update is called once per frame
    void Update()
    {
        if (isLoad)
        {
            IsLoadDoor();
        }
        else if (!isLoad)
        {           
            IsntLoadDoor();
        }
    }

    public void OpenDoor()
    {
        AudioManager.audioManager.DoorAudio.clip = AudioManager.audioManager.doorOpening;
        AudioManager.audioManager.DoorAudio.PlayOneShot(AudioManager.audioManager.doorOpening, (AudioManager.audioManager.sfxVolume * AudioManager.audioManager.masterVolume));
        StartCoroutine(OpenDelay(openWaitTime));
    }

    public void CloseDoor(float waitTime)
    {
        AudioManager.audioManager.DoorAudio.clip = AudioManager.audioManager.doorClosing;
        AudioManager.audioManager.DoorAudio.PlayOneShot(AudioManager.audioManager.doorClosing, (AudioManager.audioManager.sfxVolume * AudioManager.audioManager.masterVolume));
        StartCoroutine(CloseDelay(waitTime));       
    }

    // --------------------------------------- is load door ------------------------------------------ \\
    void IsLoadDoor() 
    {
        countDownProgress -= Time.deltaTime;  // activates timer so the player present trigger doesn't activate immidetly

        if (countDownProgress <= 0)
        {
            canLoad = true;
            countDownProgress = 0;
        }
        else if (countDownProgress >= 0 && !isOpening && playerPresent)
        {
            Debug.Log("inside");
            StartCoroutine(OpenDelay(0f));
        }

        // door closing
        if (isOpening && playerPresent && countDownProgress <= 0) // player is inside - close immediatly
        {
            CloseDoor(0f);
        }
        else if (IsOpen() && isOpening) // door is open - wait to close
        {
            CloseDoor(closeWaitTime);
        }
        else if (playerPresent && IsClosed() && countDownProgress >= 5)
        {
            OpenDelay(0f);
        }
        else if (playerPresent && IsClosed() && isLoad && canLoad) // player is inside and the door is closed - Load scene
        {
            //Debug.Log("Load Scene");
            if (Level_Manager.LM.IsSceneOpen("Outside")) Level_Manager.LM.LoadMainHab();
            else if (Level_Manager.LM.IsSceneOpen("DemoBuildingInside")) Level_Manager.LM.LoadOutside();
        }
    }

    // ------------------------------------- isn't load door ----------------------------------------- \\
    void IsntLoadDoor()
    {

    }

    // --------------------------------- Door Open / Close Checks ------------------------------------ \\
    bool IsOpen()
    {
        if (Vector3.Distance(doorRight.transform.position, rightOpenPos.transform.position) < 0.05f
            && Vector3.Distance(doorLeft.transform.position, leftOpenPos.transform.position) < 0.05f)
        {
            return true;
        }
        else return false;
    }

    bool IsClosed()
    {
        if (Vector3.Distance(doorRight.transform.position, rightClosePos) < 0.05f
            && Vector3.Distance(doorLeft.transform.position, leftClosePos) < 0.05f)
        {
            return true;
        }
        else return false;
    }

    // ---------------------------------------- Coroutines ----------------------------------------- \\
    IEnumerator OpenDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        LeanTween.move(doorRight, rightOpenPos.transform.position, 2f);
        LeanTween.move(doorLeft, leftOpenPos.transform.position, 2f);
        isOpening = true;
    }

    IEnumerator CloseDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isOpening = false;
        LeanTween.move(doorRight, rightClosePos, 2f);
        LeanTween.move(doorLeft, leftClosePos, 2f);
    }

    void Timer(float countDownProgress)
    {

    }
}
