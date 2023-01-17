using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Open / Close Bools")]
    public bool isDoorOpening;
    public bool isDoorOpenable;
    public bool isPlayerInside;
    public bool noLoad;

    [Header("Serialized Variables")]
    [SerializeField]
    float speed = 0.35f;
    [SerializeField]
    float maxTimeOpen = 13f;
    float countDownProgress;
    [SerializeField]
    float timeToOpen = 10f;

    [Header("Other Door")]
    public Door otherDoor;

    Vector3 closePos;
    public GameObject openPos;

    // Start is called before the first frame update
    void Start()
    {
        countDownProgress = maxTimeOpen;
        closePos = transform.position;       
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(isDoorOpening);
        if (isDoorOpening == true) MoveDoor(openPos.transform.position); // I'll probably LeanTween this later
        else if (!isDoorOpening) MoveDoor(closePos);

        if (isPlayerInside && Vector3.Distance(transform.position, closePos) < 0.05f)
        {
            isPlayerInside = false;

            if (!noLoad)
            {
                Debug.Log("LOAD SCENE"); // Load a new Scene here
                Level_Manager.LM.LoadMainHab();
            }
            else if (noLoad)
            {
                Debug.Log("NO LOAD, CYCLING AIRLOCK");
                otherDoor.isDoorOpening = true;
            }
        }      
    }

    // -------------------------------------- Open Door ----------------------------------------- \\

    // the OpenDoor() Meathod is called from other scripts to start the 
    // open door process
    public void OpenDoor()
    {
        // if is interacted with set isDoorOpen to true
        if (isDoorOpening == false && isDoorOpenable == true)
        {
            StartCoroutine(OpenDoorDelay());
        }
    }

    // ----------------------------------- cycle the airlock ------------------------------------- \\

    void MoveDoor(Vector3 position)
    {       
        float distance = Vector3.Distance(transform.position, position);

        if (distance > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
        }
        else if (isPlayerInside && isDoorOpening)
        {
            CountDown(0f);
        }
        else if (distance < 0.05f && isDoorOpening) // if player doesn't enter airlock auto close it
        {
            CountDown(8f);
        }
    }

    void CountDown(float waitTime) // counts down how long the airlock will stay open for
    {
        countDownProgress -= Time.deltaTime;
        if (countDownProgress <= waitTime)
        {
            countDownProgress = maxTimeOpen;
            Debug.Log("Countdown setting isDoorOpening False");
            isDoorOpening = false;                         
        }
    }

    IEnumerator OpenDoorDelay()
    {
        Debug.Log("Door Opening...");
        yield return new WaitForSeconds(timeToOpen);
        Debug.Log("Door delay coroutine setting isDoorOpening True");
        isDoorOpening = true;
    }
}
