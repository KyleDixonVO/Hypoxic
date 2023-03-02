using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnderwaterHorror;

namespace UnderwaterHorror
{
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
        public bool isOpenable = true;
        public bool playerPresent = false;
        [SerializeField] bool isLoad;
        bool canLoad;
        public bool timerActive = false;
        [SerializeField] Objective_Manager.Objectives objectiveToUnlock;

        [Header("Values")]
        [SerializeField] float openWaitTime = 10f;
        [SerializeField] float closeWaitTime = 20f;
        float countDownProgress = 5f;
        public float timeStamp;

        AudioSource airlockAudioSource;
        // Start is called before the first frame update
        void Start()
        {
            // Set close pos
            rightClosePos = doorRight.transform.position;
            leftClosePos = doorLeft.transform.position;

            // set variables
            if (!isLoad) countDownProgress = 0;
            if (otherDoor != null) otherAirlock = otherDoor.GetComponent<Airlock>();

            airlockAudioSource = this.gameObject.GetComponent<AudioSource>();
        }

        // --------------------------------------- Door Controls --------------------------------------- \\
        // Update is called once per frame
        void Update()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay)
            {
                AudioManager.audioManager.PauseSound(airlockAudioSource);
            }
            else if (GameManager.gameManager.gameState == GameManager.gameStates.gameplay && airlockAudioSource.isPlaying == false) airlockAudioSource.UnPause();

            UnlockWhenConditionMet();

            if (isLoad && isOpenable)
            {
                IsLoadDoor();
            }
            else if (!isLoad && isOpenable)
            {           
                IsntLoadDoor();
            }

            //-TIMER-------------------------------------------------------\\
            if (timerActive)
            {
                timeStamp += Time.deltaTime;
                if (IsClosed())
                {
                    // Tobias
                    this.airlockAudioSource.loop = true;
                    AudioManager.audioManager.PlaySound(airlockAudioSource, AudioManager.audioManager.doorOpening);
                    //-----------------------------------------------------------------------------------------
                }

                if (timeStamp >= openWaitTime)
                {
                    AudioManager.audioManager.StopSound(airlockAudioSource);
                    if (isOpening)
                    {
                        OpenDoor();
                        timerActive = false;
                        timeStamp = 0;
                    }
                    else if (!isOpening)
                    {
                        CloseDoor();
                        timerActive = false;
                        timeStamp = 0;
                    }
                }         
            }
            //-------------------------------------------------------TIMER-\\
        }

        public void OpenDoor()
        {
            OpenAirlock();
        }

        public void CloseDoor()
        {
            timeStamp = 0;
            CloseAirlock();
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
            else if (countDownProgress >= 0 && !isOpening && playerPresent) // on load open
            {
                //Debug.Log("inside");
                OpenAirlock();
            }

            // door closing
            if (isOpening && playerPresent && countDownProgress <= 0 && !IsClosed()) // player is inside - close immediatly
            {
                CloseDoor();
                //Debug.LogWarning("Case 1");
            }
            else if (IsOpen() && isOpening) // door is open - wait to close
            {
                timerActive = true;
                //Debug.LogWarning("Case 2");
            }
            else if (playerPresent && IsClosed() && countDownProgress >= 5)
            {
                OpenAirlock();
                //Debug.LogWarning("Case 3");
            }
            else if (playerPresent && IsClosed() && isLoad && canLoad && countDownProgress <= 0) // player is inside and the door is closed - Load scene
            {
                //Debug.Log("Load Scene");
                UI_Manager.ui_Manager.FadeOut();
                //Debug.LogWarning("Case 4");
            }
        }

        // ------------------------------------- isn't load door ----------------------------------------- \\
        void IsntLoadDoor()
        {

            // door closing
            if (isOpening && playerPresent && !IsClosed()) // player is inside - close immediatly
            {
                CloseDoor();
                Debug.LogWarning("Case 1");
            }
            else if (!IsClosed() && !playerPresent && timerActive == false) // door is open - wait to close
            {
                timerActive = true;
                Debug.LogWarning("Case 2");
            }
            else if (playerPresent && IsClosed() && otherAirlock.isOpening == false)
            {
                OpenAirlock();
                Debug.LogWarning("Case 3");
            }
            else if (playerPresent && IsClosed() && isLoad && canLoad && otherAirlock.isOpening == false) // player is inside and the door is closed - Open other door
            {
                otherAirlock.isOpening = true;
                otherAirlock.timerActive = true;
                Debug.LogWarning("Case 4");
            }
        }

        // --------------------------------- Door Open / Close Checks ------------------------------------ \\
        public bool IsOpen()
        {
            if (Vector3.Distance(doorRight.transform.position, rightOpenPos.transform.position) < 0.05f
                && Vector3.Distance(doorLeft.transform.position, leftOpenPos.transform.position) < 0.05f)
            {          
                return true;
            }
            else return false;
        }
        public bool IsClosed()
        {
            if (Vector3.Distance(doorRight.transform.position, rightClosePos) < 0.05f
                && Vector3.Distance(doorLeft.transform.position, leftClosePos) < 0.05f)
            {                
                return true;
            }
            else return false;
        }

        // --------------------------------------------- Timers -------------------------------------------- \\
        void CloseAirlock()
        {
            if (!IsClosed())
            {
                Debug.LogError("Close Door");
                LeanTween.move(doorRight, rightClosePos, 2f);
                LeanTween.move(doorLeft, leftClosePos, 2f);
                // Tobias
                AudioManager.audioManager.StopSound(airlockAudioSource);
                AudioManager.audioManager.PlaySound(airlockAudioSource, AudioManager.audioManager.doorClosed);
                airlockAudioSource.loop = false;
                //-----------------------------------------------------------------------------------------
                isOpening = false;
            }
        }

        void OpenAirlock()
        {
            if (IsClosed())
            {
                Debug.LogError("Open Door");
                LeanTween.move(doorRight, rightOpenPos.transform.position, 2f);
                LeanTween.move(doorLeft, leftOpenPos.transform.position, 2f);
            }
        }

        void UnlockWhenConditionMet()
        {
            if (isOpenable) return;
            if (Objective_Manager.objective_Manager.GetObjectiveState(objectiveToUnlock)) isOpenable = true;
        }
    }
}
