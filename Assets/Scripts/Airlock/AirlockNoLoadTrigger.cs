using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirlockNoLoadTrigger : MonoBehaviour
{
    public Airlock exteriorAirlock;
    public Airlock interiorAirlock;

    private void OnTriggerEnter(Collider other) // why did it take me so long figure this out... - Edmund
    {
        if (other.tag == "Player") // detects which door is open and which is closed and opens/closes accordingly
                                   // assumes that one is closed and one is open, God knows what happens if that isn't the case
                                   // just don't let it happen. - Edmund
        {
            //Debug.Log("player present");
            if (exteriorAirlock.isOpening)
            {
                exteriorAirlock.CloseDoor(0f);
                interiorAirlock.OpenDoor();
            }
            else if (interiorAirlock.isOpening)
            {
                interiorAirlock.CloseDoor(0f);
                exteriorAirlock.OpenDoor();
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (exteriorAirlock.isOpening)
            {
                exteriorAirlock.CloseDoor(2f);
            }
            else if (interiorAirlock.isOpening)
            {
                interiorAirlock.CloseDoor(2f);
            }
        }

    }
}
