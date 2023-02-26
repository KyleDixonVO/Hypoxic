using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror 
{
    public class SubmenuButton : MonoBehaviour
    {
        public static SubmenuButton caller;
        public UI_Manager.PDASubmenu activeSubmenu;
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


