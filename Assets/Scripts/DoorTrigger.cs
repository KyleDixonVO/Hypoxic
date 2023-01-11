using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject airlock;
    Door exteriorDoor;

    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        exteriorDoor = airlock.GetComponent<Door>();
    }

    // Update is called once per frame
    void Update()
    {
        //TEMP Close
        /*if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log("player is in");
            exteriorDoor.isPlayerInside = true;
        }*/
        timer += Time.deltaTime;
        Debug.LogWarning(timer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && timer > 5f)
        {
            exteriorDoor.isPlayerInside = true;
            timer = 0;
        }
        else if (timer < 5f)
        {
            exteriorDoor.isDoorOpening = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        exteriorDoor.isPlayerInside = false;
    }
}
