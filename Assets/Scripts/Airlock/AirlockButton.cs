using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirlockButton : MonoBehaviour
{
    public GameObject Door;
    [SerializeField]
    Airlock AL;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKey(KeyCode.E) && !AL.isOpening)
        {
            Debug.Log("Button pressed");
            AL.OpenDoor();
        }

    }
}
