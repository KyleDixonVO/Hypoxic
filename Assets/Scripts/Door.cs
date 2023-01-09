using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isDoorOpen;
    public bool isDoorOpenable;

    float speed = 0.25f;

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
        // if is interacted with run OpendDoor()

        //TEMP
        if (Input.GetMouseButtonDown(0) && isDoorOpen == false && isDoorOpenable == true)
        {
            isDoorOpen = true;
        }

        if (isDoorOpen == true) OpenDoor();
        
    }

    void OpenDoor()
    {
        // open door

        Debug.Log("Open");
        float distance = Vector3.Distance(transform.position, openPos);
        if (distance > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, openPos, speed * Time.deltaTime);
        }
    }

    void CloseDoor()
    {

    }
}
