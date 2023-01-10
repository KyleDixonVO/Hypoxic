using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject airlock;
    Door exteriorDoor;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            exteriorDoor.isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        exteriorDoor.isPlayerInside = false;
    }
}
