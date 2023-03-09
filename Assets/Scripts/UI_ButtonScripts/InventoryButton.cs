using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnderwaterHorror 
{
    public class InventoryButton : MonoBehaviour
    {
        public static InventoryButton caller;
        public Image border;
        public int slot;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (caller == null || caller != this)
            {
                border.gameObject.SetActive(false);
            }
            else
            {
                border.gameObject.SetActive(true);
            }
        }

        public void SetThisAsCaller()
        {
            caller = this;
        }
        
    }
}


