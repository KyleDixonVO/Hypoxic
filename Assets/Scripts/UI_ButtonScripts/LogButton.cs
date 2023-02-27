using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror 
{
    public class LogButton : MonoBehaviour
    {
        public static LogButton caller;
        public UI_Manager.Log activeLog;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetThisAsCaller()
        {
            caller = this;
        } 
    }
}


