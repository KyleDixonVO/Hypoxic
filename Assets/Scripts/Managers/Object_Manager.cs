using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class Object_Manager : MonoBehaviour
    {
        public static Object_Manager object_Manager;
        [SerializeField] HeavyObject[] heavyObjects;
        private void Awake()
        {
            if (object_Manager == null)
            {
                object_Manager = this;
                DontDestroyOnLoad(this);
            }
            else if (object_Manager != null && object_Manager != this)
            {
                Destroy(this);
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            heavyObjects = new HeavyObject[Data_Manager.dataManager.numberOfObjectives - 2];
        }

        // Update is called once per frame
        void Update()
        {
            HeavyObjectSingleton();
            ToggleRigidbodies();
            if (FirstPersonController_Sam.fpsSam == null || heavyObjects.Length == 0) return;
            WithinPickupRange();
        }

        public bool WithinPickupRange()
        {
            for (int i = 0; i < GameObject.FindObjectsOfType<HeavyObject>().Length; i++)
            {
                if (GameObject.FindObjectsOfType<HeavyObject>()[i].WithinPickupRange()) return true;   
            }
            return false;
        }

        public void HeavyObjectSingleton()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
            for (int i = 0; i < heavyObjects.Length; i++)
            {
 
                switch (i) 
                {
                    case 0:
                        if (heavyObjects[i] == null)
                        {
                            heavyObjects[i] = GameObject.Find("pipeFixedRed").GetComponent<HeavyObject>();
                            DontDestroyOnLoad(heavyObjects[i]);
                        }
                        else if (heavyObjects[i] != null && heavyObjects[i] != GameObject.Find("pipeFixedRed").GetComponent<HeavyObject>())
                        {
                            Destroy(GameObject.Find("pipeFixedRed"));
                        }
                        break;

                    case 1:
                        if (heavyObjects[i] == null)
                        {
                            heavyObjects[i] = GameObject.Find("pipeFixedGreen").GetComponent<HeavyObject>();
                            DontDestroyOnLoad(heavyObjects[i]);
                        }
                        else if (heavyObjects[i] != null && heavyObjects[i] != GameObject.Find("pipeFixedGreen").GetComponent<HeavyObject>())
                        {
                            Destroy(GameObject.Find("pipeFixedGreen"));
                        }
                        break;

                    case 2:
                        break;
                }

                
            }
        }

        private void ToggleRigidbodies()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
            if (Level_Manager.LM.IsSceneOpen("Outside"))
            {
                for (int i = 0; i < heavyObjects.Length; i++)
                {
                    if (heavyObjects[i].GetComponent<Rigidbody>().IsSleeping() == false) return;
                    heavyObjects[i].GetComponent<Rigidbody>().WakeUp();
                }
            }
            else
            {
                for (int i = 0; i < heavyObjects.Length; i++)
                {
                    if (heavyObjects[i].GetComponent<Rigidbody>().IsSleeping() == true) return;
                    heavyObjects[i].GetComponent<Rigidbody>().Sleep();
                }
            }
        }
    }

}
