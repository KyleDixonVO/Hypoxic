using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror 
{
    public class Interactable_Manager : MonoBehaviour
    {
        [SerializeField] private bool useRandomSpawns;
        [SerializeField] private bool clearedJunk = false;
        public static Interactable_Manager interactable_manager;
        public Interactable[] interactables;
        public string[] itemNames;
        [SerializeField] private bool spawnedItems;
        [SerializeField] private int maxItems;
        [SerializeField] private int itemRNGCeiling;
        [SerializeField] private GameObject[] spawnPoints;
        [SerializeField] private GameObject[] itemPrefabs;
        [SerializeField] Vector3[] startingPositions;
        private List<int> occupiedSpawns;


        private void Awake()
        {
            if (interactable_manager == null)
            {
                interactable_manager = this;
                DontDestroyOnLoad(this);
            }
            else if (interactable_manager != null && interactable_manager != this)
            {
                Destroy(this.gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (useRandomSpawns)
            {
                interactables = new Interactable[maxItems];
            }
            else
            {
                interactables = new Interactable[itemNames.Length];
                StaticSpawnInteractables();
            }
        }

        // Update is called once per frame
        void Update()
        {
            ItemSingleton();
            ToggleRigidbodies();
        }

        void ItemSingleton()
        {
            //if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
            for (int i = 0; i < interactables.Length; i++)
            {
                if (interactables[i] == null)
                {
                    try
                    {
                        interactables[i] = GameObject.Find(itemNames[i]).GetComponent<Interactable>();
                        DontDestroyOnLoad(interactables[i]);
                        interactables[i].singleton = true;
                        Debug.Log("Found " + interactables[i]);
                    }
                    catch
                    {
                        Debug.Log("Cannot find an instance of " + itemNames[i]);
                    }
                }
                else if (!interactables[i].singleton)
                {
                    DontDestroyOnLoad(interactables[i]);
                    interactables[i].singleton = true;
                }


                if (interactables[i].GetComponent<Item>() == null) continue;
                if (!interactables[i].GetComponent<Item>().isUsed) continue;
                //Debug.Log(i);
                interactables[i].gameObject.SetActive(false);
                
            }
        }

        void RemoveDuplicates()
        {
            if (clearedJunk) return;
            Debug.Log("Clearing Junk");
            for (int i = 0; i < GameObject.FindObjectsOfType<Interactable>().Length; i++)
            {
                //Debug.Log(i);
                if (GameObject.FindObjectsOfType<Interactable>()[i].GetComponent<HeavyObject>() != null || GameObject.FindObjectsOfType<Interactable>()[i].GetComponent<RepairTarget>() != null || GameObject.FindObjectsOfType<Interactable>()[i].GetComponent<AirlockButton>() != null || GameObject.FindObjectsOfType<Interactable>()[i].GetComponent<SaveStation>() != null) continue;
                if (GameObject.FindObjectsOfType<Interactable>()[i].singleton == false)
                {
                    Destroy(GameObject.FindObjectsOfType<Interactable>()[i].gameObject);
                    Debug.LogWarning("Destroyed Duplicate Item!");
                }
            }
            clearedJunk = true;
        }

        public void SetClearedJunkFalse()
        {
            clearedJunk = false;
        }

        private void ToggleRigidbodies()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
            for (int i = 0; i < interactables.Length; i++)
            {
                if (interactables[i] == null) continue;
                if (interactables[i].GetComponent<Rigidbody>() == null) return;
                if (Level_Manager.LM.IsSceneOpen("Outside") && interactables[i].GetComponent<Rigidbody>().IsSleeping() == true)
                {
                    interactables[i].GetComponent<Rigidbody>().WakeUp();
                }
                else if (!Level_Manager.LM.IsSceneOpen("Outside") && interactables[i].GetComponent<Rigidbody>().IsSleeping() == false)
                {
                    interactables[i].GetComponent<Rigidbody>().Sleep();
                }
            }
        }

        public void SetSavePositions()
        {
            for (int i = 0; i < interactables.Length; i++)
            {
                interactables[i].SetSaveGamePos();
            }

            Data_Manager.dataManager.InteractableManagerToDataManager();
        }

        public void ReloadToSavePositions()
        {
            Debug.Log("Resetting to save positions");
            for (int i = 0; i < interactables.Length; i++)
            {
                interactables[i].ReloadToSave();
            }
        }

        public void ResetForNewRun()
        {
            for (int i = 0; i < interactables.Length; i++)
            {
                interactables[i].ResetForNewRun();
            }

            spawnedItems = false;
        }

        void RandomSpawnInteractables()
        {
            int itemRNG;
            int spawnRNG;
            if (spawnedItems != false) return;
            for (int i = 0; i < maxItems; i++)
            {
                itemRNG = Random.Range(0, itemRNGCeiling);
                spawnRNG = Random.Range(0, spawnPoints.Length - 1);
                while (occupiedSpawns.Contains(spawnRNG))
                {
                    spawnRNG = Random.Range(0, spawnPoints.Length - 1);
                }

                switch (itemRNG)
                {
                    case <= 20:
                        break;

                    case <= 40:
                        break;

                    case <= 60:
                        break;

                    case <= 80:
                        break;

                    case <= 100:
                        break;
                }


            }

        }

        void StaticSpawnInteractables()
        {
            int prods = 1;
            int batteries = 1;
            int glowsticks = 1;
            for (int i = 0; i < interactables.Length; i++)
            {

                switch (i)
                {
                    case 0:
                        //gun
                        interactables[i] = Instantiate(itemPrefabs[0], parent: this.gameObject.transform, false).GetComponent<Interactable>();
                        interactables[i].name = "Gun";
                        interactables[i].transform.position = startingPositions[i];
                        break;

                    case 1:
                    case 10:
                        //prod
                        interactables[i] = Instantiate(itemPrefabs[1], parent: this.gameObject.transform, false).GetComponent<Interactable>();
                        interactables[i].name = ("Shock_Prod_" + prods);
                        interactables[i].transform.position = startingPositions[i];
                        prods++;
                        break;

                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        //battery
                        interactables[i] = Instantiate(itemPrefabs[2], parent: this.gameObject.transform, false).GetComponent<Interactable>();
                        interactables[i].name = ("Battery_" + batteries);
                        interactables[i].transform.position = startingPositions[i];
                        batteries++;
                        break;

                    case 14:
                        //Medkit
                        interactables[i] = Instantiate(itemPrefabs[3], parent: this.gameObject.transform, false).GetComponent<Interactable>();
                        interactables[i].name = ("Medkit");
                        interactables[i].transform.position = startingPositions[i];
                        break;

                    default:
                        //glowstick
                        interactables[i] = Instantiate(itemPrefabs[4], parent: this.gameObject.transform, false).GetComponent<Interactable>();
                        interactables[i].name = ("GlowStick_" + glowsticks);
                        interactables[i].transform.position = startingPositions[i];
                        glowsticks++;
                        break;
                }

                

            }
        }
    }
}


