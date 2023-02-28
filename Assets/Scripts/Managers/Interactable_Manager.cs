using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror 
{
    public class Interactable_Manager : MonoBehaviour
    {
        [SerializeField] private bool useRandomSpawns;
        public static Interactable_Manager item_manager;
        public Interactable[] interactables;
        public string[] itemNames;
        [SerializeField] private bool spawnedItems;
        [SerializeField] private int maxItems;
        [SerializeField] private int itemRNGCeiling;
        [SerializeField] private GameObject[] spawnPoints;
        private List<int> occupiedSpawns;


        private void Awake()
        {
            if (item_manager == null)
            {
                item_manager = this;
                DontDestroyOnLoad(this);
            }
            else if (item_manager != null && item_manager != this)
            {
                Destroy(this);
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
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
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
                        Debug.Log("Cannot find an instance of " + interactables[i]);
                    }
                }
            }

            for (int i = 0; i < GameObject.FindObjectsOfType<Interactable>().Length; i++)
            {
                if (!GameObject.FindObjectsOfType<Interactable>()[i].GetComponent<Interactable>().singleton)
                {
                    Destroy(GameObject.FindObjectsOfType<Interactable>()[i]);
                }
            }
        }

        private void ToggleRigidbodies()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
            for (int i = 0; i < interactables.Length; i++)
            {
                if (interactables[i] == null) continue;
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
                if (interactables[i].GetComponent<Item>()) interactables[i].GetComponent<Item>().SetSaveGamePos();
                else if (interactables[i].GetComponent<Weapon>()) interactables[i].GetComponent<Weapon>().SetSaveGamePos();
            }

            Data_Manager.dataManager.EnemyManagerToDataManager();
        }

        void SpawnInteractables()
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
    }
}

