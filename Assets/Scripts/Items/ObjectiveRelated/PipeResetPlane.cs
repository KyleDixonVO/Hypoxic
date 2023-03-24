using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class PipeResetPlane : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Pipe")
            {
                Debug.Log("Pipe fell out of map");
                other.GetComponent<HeavyObject>().ForceDropObject();
                other.GetComponent<HeavyObject>().ResetForNewRun();
                
            }
        }
    }
}

