using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class Object_Manager : MonoBehaviour
    {
        public static Object_Manager object_Manager;
        [SerializeField] HeavyObject[] heavyObjects;
        [SerializeField] string[] heavyObjectNames;
        [SerializeField] RepairTarget[] repairTargets;
        private bool goneInside;

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
            heavyObjects = new HeavyObject[heavyObjectNames.Length];
            repairTargets = new RepairTarget[heavyObjectNames.Length];
        }

        // Update is called once per frame
        void Update()
        {
            HeavyObjectSingleton();
            ToggleRigidbodies();
            if (FirstPersonController_Sam.fpsSam == null || heavyObjects.Length == 0) return;
            WithinPickupRange();
            if (!Level_Manager.LM.IsSceneOpen("Outside")) 
            {
                goneInside = true;
                return;
            } 
            else if (goneInside && Level_Manager.LM.IsSceneOpen("Outside"))
            {
                SetRepairPipeStatus();
                goneInside = false;
            }
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
                if(heavyObjects[i] == null)
                {
                    try
                    {
                        heavyObjects[i] = GameObject.Find(heavyObjectNames[i]).GetComponent<HeavyObject>();
                        DontDestroyOnLoad(heavyObjects[i]);
                        heavyObjects[i].singleton = true;
                        //Debug.Log("Found " + heavyObjectNames[i]);
                        
                    }
                    catch
                    {
                        Debug.Log("Cannot find an instance of " + heavyObjectNames[i]);
                    }
                }
            }

            for (int i = 0; i < GameObject.FindGameObjectsWithTag("Pipe").Length; i++)
            {
                if (!GameObject.FindGameObjectsWithTag("Pipe")[i].GetComponent<HeavyObject>().singleton)
                {
                    Destroy(GameObject.FindGameObjectsWithTag("Pipe")[i]);
                }
            }
        }

        private void ToggleRigidbodies()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
            for (int i = 0; i < heavyObjects.Length; i++)
            {
                if (heavyObjects[i] == null) continue;
                if (Level_Manager.LM.IsSceneOpen("Outside") && heavyObjects[i].GetComponent<Rigidbody>().IsSleeping() == true)
                {
                    heavyObjects[i].GetComponent<Rigidbody>().WakeUp();
                }
                else if (!Level_Manager.LM.IsSceneOpen("Outside") && heavyObjects[i].GetComponent<Rigidbody>().IsSleeping() == false)
                {
                    heavyObjects[i].GetComponent<Rigidbody>().Sleep();
                }
            }
        }

        public void SetSavePositions()
        {
            for (int i = 0; i < heavyObjects.Length; i++)
            {
                if (heavyObjects[i] == null) continue;
                heavyObjects[i].SetSavePos();
            }
        }

        public void ReturnToSavedPositions()
        {
            for (int i = 0; i < heavyObjects.Length; i++)
            {
                if (heavyObjects[i] == null) continue;
                heavyObjects[i].ReturnToSavePos();
            }
        }

        public void ResetForNewRun()
        {
            for (int i = 0; i < heavyObjects.Length; i++)
            {
                if (heavyObjects[i] == null) continue;
                heavyObjects[i].ResetToStartingPosition();
            }
        }

        void SetRepairPipeStatus()
        {
            repairTargets = GameObject.FindObjectsOfType<RepairTarget>();

            if (Objective_Manager.objective_Manager.GetObjectiveState(Objective_Manager.Objectives.repairRedPipe) && repairTargets[0] != null) repairTargets[0].RepairedObject();
            if (Objective_Manager.objective_Manager.GetObjectiveState(Objective_Manager.Objectives.repairGreenPipe) && repairTargets[1] != null) repairTargets[1].RepairedObject();
            if (Objective_Manager.objective_Manager.GetObjectiveState(Objective_Manager.Objectives.repairTutorialPipe) && repairTargets[2] != null) repairTargets[2].RepairedObject();
        }
    }

}
