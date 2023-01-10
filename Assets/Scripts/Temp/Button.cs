using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject door;
    private Door doorScript;

    // This is bad and TEMP,
    // basically when the player is in this trigger and presses
    // 'E' it activates the airlock cycle, starting the open door process
    // - Edmund
    // 

    void Start()
    {
        doorScript = door.GetComponent<Door>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            Debug.LogWarning("Press");
            doorScript.OpenDoor();
        }
    }
}
