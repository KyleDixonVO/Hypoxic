using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject door;
    private Door doorScript;
    public bool openDoor;
    public Level_Manager LM;

    // This is bad and TEMP,
    // basically when the player is in this trigger and presses
    // 'E' it activates the airlock cycle, starting the open door process
    // - Edmund
    // 

    void Start()
    {
        doorScript = door.GetComponent<Door>();
    }

    private void OnTriggerStay(Collider other) // while in trigger if 'E' is pressed do the thing
    {
        if (!openDoor)
        {
            if (other.tag == "Player" && Input.GetKey(KeyCode.E))
            {
                Debug.LogWarning("Press");
                doorScript.OpenDoor();
            }
        }
        else
        {
            if (other.tag == "Player" && Input.GetKey(KeyCode.E) && doorScript.isDoorOpening == false)
            {
                Debug.LogWarning("LoadScene");
                LM.LoadOutside();
            }
        }

    }
}
