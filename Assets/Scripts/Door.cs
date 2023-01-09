using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Open / Close Bools")]
    public bool isDoorOpening;
    public bool isDoorOpenable;
    public bool isPlayerInside;

    [Header("Serialized Variables")]
    [SerializeField]
    float speed = 0.35f;
    [SerializeField]
    float maxTimeOpen = 13f;
    float timeOpen;

    Vector3 closePos;
    Vector3 openPos;

    // Start is called before the first frame update
    void Start()
    {
        closePos = transform.position;
        openPos = new Vector3(transform.position.x + 2.5f, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // if is interacted with set isDoorOpen to true
        // TEMP opening with Left Mouse
        if (Input.GetMouseButtonDown(0) && isDoorOpening == false && isDoorOpenable == true)
        {
            isDoorOpening = true;
        }

        if (isDoorOpening == true) MoveDoor(openPos); // I'll probably LeanTween this later
        else if (!isDoorOpening) MoveDoor(closePos);

        if (isPlayerInside && Vector3.Distance(transform.position, closePos) < 0.1f)
        {
            Debug.Log("LOAD SCENE");
            isPlayerInside = false;
        }
        
    }

    // ----------------------------------- cycle the airlock ------------------------------------- \\

    void MoveDoor(Vector3 position)
    {       
        float distance = Vector3.Distance(transform.position, position);

        if (distance > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
        }
        else if (isPlayerInside && isDoorOpening)
        {
            CountDown(5f);
            Debug.Log("CLOSING W/ PLAYER");
        }
        else if (distance < 0.1f && isDoorOpening) // if player doesn't enter airlock auto close it
        {
            Debug.Log("CLOSING");
            CountDown(0f);
        }
    }

    void CountDown(float maxTime)
    {
        timeOpen -= Time.deltaTime;
        if (timeOpen <= maxTime)
        {
            timeOpen = maxTimeOpen;
            isDoorOpening = false;          
        }
    }
}
