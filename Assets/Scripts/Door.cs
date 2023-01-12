using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("LevelManager")]
    public Level_Manager LM;

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
    float timeOpen;
    [SerializeField]
    float timeToOpen = 10f;

    [Header("Other Door")]
    public Door otherDoor;

    Vector3 closePos;
    Vector3 openPos;

    // Start is called before the first frame update
    void Start()
    {
        timeOpen = maxTimeOpen;
        closePos = transform.position;
        openPos = new Vector3(transform.position.x + 4.5f, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDoorOpening == true) MoveDoor(openPos); // I'll probably LeanTween this later
        else if (!isDoorOpening) MoveDoor(closePos);

        if (isPlayerInside && Vector3.Distance(transform.position, closePos) < 0.05f)
        {
            Debug.Log("LOAD SCENE"); // Load a new Scene here
            isPlayerInside = false;

            if (!noLoad)LM.LoadMainHab();
            else if (noLoad)
            {
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
            CountDown(8f);
        }
        else if (distance < 0.05f && isDoorOpening) // if player doesn't enter airlock auto close it
        {
            CountDown(0f);
        }
    }

    void CountDown(float waitTimeReduction) // counts down how long the airlock will stay open for
    {
        timeOpen -= Time.deltaTime;
        if (timeOpen <= waitTimeReduction)
        {
            timeOpen = maxTimeOpen;
            isDoorOpening = false;                         
        }
    }    

    IEnumerator OpenDoorDelay()
    {
        Debug.Log("Door Opening...");
        yield return new WaitForSeconds(timeToOpen);
        isDoorOpening = true;
    }
}
