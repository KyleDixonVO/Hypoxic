using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirlockMini : MonoBehaviour
{
    public GameObject door;
    bool canOpen;
    public bool buttonPress;
    int timeToOpen = 4;

    // Start is called before the first frame update
    void Start()
    {
        canOpen = true;
        buttonPress = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonPress && canOpen)
        {
            OnOpen();
            canOpen = false;
        }
    }

    void OnOpen()
    {
        LeanTween.rotateLocal(door, new Vector3(0, 105, 0), timeToOpen);
    }
}
