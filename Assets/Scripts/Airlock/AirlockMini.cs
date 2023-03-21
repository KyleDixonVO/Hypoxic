using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class AirlockMini : MonoBehaviour
    {
        public GameObject door;
        bool canOpen;
        public bool buttonPress;
        int timeToOpen = 4;
        [SerializeField] AudioSource source;

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
            AudioManager.audioManager.PlaySound(source, AudioManager.audioManager.doorOpened);
            LeanTween.rotateLocal(door, new Vector3(0, 105, 0), timeToOpen);
        }
    }
}

