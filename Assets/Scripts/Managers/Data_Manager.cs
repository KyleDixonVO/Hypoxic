using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace UnderwaterHorror
{
    public class Data_Manager : MonoBehaviour
    {
        public static Data_Manager dataManager;

        public bool saving;

        //global prefs
        public float mastervolume;
        public float musicVolume;
        public float SFXVolume;

        //playerStats data
        public float maxSuitPower;
        public float suitPower;
        public float maxPlayerHealth;
        public float playerHealth;

        //objectiveManager data
        public bool[] objectives;

        //firstPersonControllerSam data
        public bool inWater;
        public bool carryingHeavyObj;

        //vector3 floats
        public SerializableVector3 playerSV = new SerializableVector3();

        //quaternion floats
        public SerializableQuaternion playerSQ = new SerializableQuaternion();

        //enemyManager
        public int numberOfEnemies;
        [SerializeField] Enemy[] enemies;

        //interactableManager
        public int numberOfItems;
        [SerializeField] GameObject[] items;
        [SerializeField] GameObject[] itemPrefabs;
        [SerializeField] GameObject[] heldItems;




        void Awake()
        {
            if (dataManager == null)
            {
                dataManager = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (dataManager != this)
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            PopulateArrays();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void PopulateArrays()
        {
            heldItems = new GameObject[3];
            numberOfItems = Interactable_Manager.interactable_manager.itemNames.Length;
            numberOfEnemies = Enemy_Manager.enemy_Manager.enemyNames.Length;
            objectives = new bool[Enum.GetNames(typeof(Objective_Manager.Objectives)).Length];
            enemies = new Enemy[numberOfEnemies];
            for (int i = 0; i < heldItems.Length; i++)
            {
                GameObject temp = new GameObject();
                heldItems[i] = temp;
            }
            for (int i = 0; i < enemies.Length; i++)
            {
                GameObject temp = new GameObject();
                temp.AddComponent<Enemy>();
                temp.AddComponent<EnemyStats>();
                temp.GetComponent<Enemy>()._enemyStats = temp.GetComponent<EnemyStats>();
                enemies[i] = temp.GetComponent<Enemy>();
                enemies[i].singleton = true;
                enemies[i].transform.parent = this.gameObject.transform;
            }

            items = new GameObject[numberOfItems];
            int prods = 1;
            int batteries = 1;
            int glowsticks = 1;
            for (int i = 0; i < items.Length; i++)
            {

                switch (i)
                {
                    case 0:
                        //gun
                        items[i] = Instantiate(itemPrefabs[0], parent: this.gameObject.transform, false);
                        items[i].name = "Gun";
                        items[i].SetActive(false);
                        break;

                    case 1:
                    case 10:
                        //prod
                        items[i] = Instantiate(itemPrefabs[1], parent: this.gameObject.transform, false);
                        items[i].name = ("Shock_Prod_" + prods);
                        items[i].SetActive(false);
                        prods++;
                        break;

                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        //battery
                        items[i] = Instantiate(itemPrefabs[2], parent: this.gameObject.transform, false);
                        items[i].name = ("Battery_" + batteries);
                        items[i].SetActive(false);
                        batteries++;
                        break;

                    case 14:
                        //Medkit
                        items[i] = Instantiate(itemPrefabs[3], parent: this.gameObject.transform, false);
                        items[i].name = ("Medkit");
                        items[i].SetActive(false);
                        break;

                    default:
                        //glowstick
                        items[i] = Instantiate(itemPrefabs[4], parent: this.gameObject.transform, false);
                        items[i].name = ("GlowStick_" + glowsticks);
                        items[i].SetActive(false);
                        glowsticks++;
                        break;
                }

            }
        }

        public bool SaveExists()
        {
            if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
            {
                return true;
            }
            return false;
        }

        // -------------------------------------------------------------------------------------------------------------------------------------------------- Global Data
        
        //saves out settings prefs
        public void SaveGlobalData()
        {
            saving = true;
            Debug.Log("Saving Global Data");
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream globalFile = File.Create(Application.persistentDataPath + "/globalData.dat");
            GlobalData globalData = new GlobalData();

            //save settings here---------------------------------
            globalData.masterVolume = mastervolume;
            globalData.musicVolume = musicVolume;
            globalData.SFXVolume = SFXVolume;
            //---------------------------------------------------

            binaryFormatter.Serialize(globalFile, globalData);
            globalFile.Close();
            saving = false;
        }

        //loads settings prefs from globalData into Data_Manager
        public void LoadGlobalData()
        {
            if (File.Exists(Application.persistentDataPath + "/globalData.dat"))
            {
                //Debug.Log("Loading Global Data");
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream globalFile = File.Open(Application.persistentDataPath + "/globalData.dat", FileMode.Open);
                GlobalData globalData = (GlobalData)binaryFormatter.Deserialize(globalFile);
                globalFile.Close();


                //load settings here
                mastervolume = globalData.masterVolume;
                musicVolume = globalData.musicVolume;
                SFXVolume = globalData.SFXVolume;
            }
            else
            {
                Debug.LogError("Error loading global data, resetting to default");
                ResetGlobalPrefs();
            }
        }

        private void ResetGlobalPrefs()
        {
            mastervolume = 1.0f;
            musicVolume = 1.0f;
            SFXVolume = 1.0f;

            SaveGlobalData();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------- Player & Objective Data

        //loads from playerData into Data_Manager
        public void LoadFromPlayerData()
        {
            if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
            {
                Debug.Log("File found, loading player data");
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream playerFile = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);
                if (playerFile.Length == 0)
                {
                    playerFile.Close();
                    Debug.Log("File length 0");
                    //set stats to default if save is missing or unreadable
                    ResetPlayerData();
                }
                else
                {
                    playerFile.Position = 0;
                    PlayerData playerData = (PlayerData)binaryFormatter.Deserialize(playerFile);
                    playerFile.Close();

                    //set local playerData vars equal to loadedPlayerData
                    maxPlayerHealth = playerData.maxHealth;
                    playerHealth = playerData.health;
                    maxSuitPower = playerData.maxSuitPower;
                    suitPower = playerData.suitPower;

                    for (int i = 0; i < Enum.GetNames(typeof(Objective_Manager.Objectives)).Length; i++)
                    {
                        objectives[i] = playerData.objectives[i];
                    }
                    //objectives[(int)Objective_Manager.Objectives.repairFirstPipe] = playerData.objectives[(int)Objective_Manager.Objectives.repairFirstPipe];
                    //objectives[(int)Objective_Manager.Objectives.repairSecondPipe] = playerData.objectives[(int)Objective_Manager.Objectives.repairSecondPipe];
                    //objectives[(int)Objective_Manager.Objectives.repairThirdPipe] = playerData.objectives[(int)Objective_Manager.Objectives.repairThirdPipe];
                    //objectives[(int)Objective_Manager.Objectives.goToElevator] = playerData.objectives[(int)Objective_Manager.Objectives.goToElevator];
                    inWater = playerData.inWater;
                    carryingHeavyObj = playerData.carryingHeavyObj;
                    RotPosToManager(playerData);
                }
            }
            else
            {
                //set stats to default if save is missing or unreadable
                Debug.Log("File error, resetting player stats to default");
                ResetPlayerData();
            }
        }

        //saves to playerData from Data_Manager
        public void SaveToPlayerData()
        {
            saving = true;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream playerFile = File.Create(Application.persistentDataPath + "/playerData.dat");
            PlayerData playerData = new PlayerData();

            playerData.maxSuitPower = maxSuitPower;
            playerData.suitPower = suitPower;
            playerData.maxHealth = maxPlayerHealth;
            playerData.health = playerHealth;
            for (int i = 0; i < Enum.GetNames(typeof(Objective_Manager.Objectives)).Length; i++)
            {
                playerData.objectives[i] = objectives[i];
            }
            //playerData.objectives[(int)Objective_Manager.Objectives.repairFirstPipe] = objectives[(int)Objective_Manager.Objectives.repairFirstPipe];
            //playerData.objectives[(int)Objective_Manager.Objectives.repairSecondPipe] = objectives[(int)Objective_Manager.Objectives.repairSecondPipe];
            //playerData.objectives[(int)Objective_Manager.Objectives.repairThirdPipe] = objectives[(int)Objective_Manager.Objectives.repairThirdPipe];
            //playerData.objectives[(int)Objective_Manager.Objectives.goToElevator] = objectives[(int)Objective_Manager.Objectives.goToElevator];
            playerData.inWater = inWater;
            playerData.carryingHeavyObj = carryingHeavyObj;
            RotPosToPlayerData(playerData);

            binaryFormatter.Serialize(playerFile, playerData);
            playerFile.Close();
            saving = false;
        }

        //saves to Data_Manager from source classes
        public void PlayerAndObjectiveDataToDataManager()
        {
            Debug.LogWarning(Enum.GetNames(typeof(Objective_Manager.Objectives)).Length);
            for (int i = 0; i < Enum.GetNames(typeof(Objective_Manager.Objectives)).Length; i++)
            {
                objectives[i] = Objective_Manager.objective_Manager.GetObjectiveState((Objective_Manager.Objectives)i);
            } 

            if (PlayerStats.playerStats == null) return;
            maxSuitPower = PlayerStats.playerStats.maxSuitPower;
            suitPower = PlayerStats.playerStats.suitPower;
            maxPlayerHealth = PlayerStats.playerStats.maxPlayerHealth;
            playerHealth = PlayerStats.playerStats.playerHealth;
            inWater = FirstPersonController_Sam.fpsSam.inWater;
            carryingHeavyObj = FirstPersonController_Sam.fpsSam.carryingHeavyObj;
            SavedPosToFloats();
            PlayerRotationToPlayerSQ();
            SaveToPlayerData();
        }

        //converts vector3 to floats for serializing
        private void SavedPosToFloats()
        {
            playerSV.SetSerializableVector(FirstPersonController_Sam.fpsSam.playerSavedPosition);
        }

        //converts floats back to vector3 to pass into fpsSam
        private void FloatsToSavedPos()
        {
            playerSV.GetSerializableVector(ref FirstPersonController_Sam.fpsSam.playerSavedPosition);
            //Debug.Log(playerSV.x + " " + playerSV.y + " " + playerSV.z);
            //Debug.Log(FirstPersonController_Sam.fpsSam.playerSavedPosition);
        }

        //converts vector4 to floats for serializing
        private void PlayerRotationToPlayerSQ()
        {
            playerSQ.SetSerializableQuaternion(FirstPersonController_Sam.fpsSam.playerSavedRotation);
        }

        //converts floats back to vector4 to pass into fpsSam
        private void PlayerSQToPlayerRotation()
        {
            playerSQ.GetSerializableQuaternion(ref FirstPersonController_Sam.fpsSam.playerSavedRotation);
        }

        //passing floats from Data_Manager to playerData
        private void RotPosToPlayerData(PlayerData playerData)
        {
            playerData.playerSQ.SetSerializableQuaternion(new Quaternion(playerSQ.x, playerSQ.y, playerSQ.z, playerSQ.w));

            playerData.playerSV.SetSerializableVector(new Vector3(playerSV.x, playerSV.y, playerSV.z));
        }

        //passing floats from playerData to Data_Manager
        private void RotPosToManager(PlayerData playerData)
        {
            playerSQ.SetSerializableQuaternion(new Quaternion(playerData.playerSQ.x, playerData.playerSQ.y, playerData.playerSQ.z, playerData.playerSQ.w));
            

            playerSV.SetSerializableVector(new Vector3(playerData.playerSV.x, playerData.playerSV.y, playerData.playerSV.z));
            //Debug.Log(playerSV.x + " " + playerSV.y + " " + playerSV.z);
        }

        private void ResetPlayerData()
        {
            //reset player stats to default and then save stats
            //place default data here
            //playerStats data
            maxSuitPower = 100.0f;
            suitPower = maxSuitPower;
            maxPlayerHealth = 100.0f;
            playerHealth = maxPlayerHealth;

            inWater = false;
            carryingHeavyObj = false;

            //fpsSam data
            if (FirstPersonController_Sam.fpsSam != null)
            {
                FirstPersonController_Sam.fpsSam.ResetRun();
            }

            //objectiveManager data
            for (int i = 0; i < Enum.GetNames(typeof(Objective_Manager.Objectives)).Length; i++)
            {
                objectives[i] = false;
            }

            SaveToPlayerData();
        }

        //passes Data_Manager references out to playerStats
        public void UpdatePlayerStats()
        {
            if (PlayerStats.playerStats == null) return;
            PlayerStats.playerStats.maxPlayerHealth = maxPlayerHealth;
            PlayerStats.playerStats.playerHealth = playerHealth;
            PlayerStats.playerStats.maxSuitPower = maxSuitPower;
            PlayerStats.playerStats.suitPower = suitPower;
        }

        //passes Data_Manager references out to Objective_Manager
        public void UpdateObjectiveManager()
        {
            for (int i = 0; i < objectives.Length; i++)
            {
                if (!objectives[i]) continue;
                Objective_Manager.objective_Manager.UpdateObjectiveCompletion(i);
            }
        }

        //passes Data_Manager references out to fpsSam
        public void UpdateFPSSam()
        {
            if (FirstPersonController_Sam.fpsSam == null) return;
            FirstPersonController_Sam.fpsSam.inWater = inWater;
            FirstPersonController_Sam.fpsSam.carryingHeavyObj = carryingHeavyObj;
            FloatsToSavedPos();
            PlayerSQToPlayerRotation();
        }

        // -------------------------------------------------------------------------------------------------------------------------------------------------- Enemy Data

        private void SaveToEnemyData()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream enemyFile = File.Create(Application.persistentDataPath + "/enemyData.dat");
            saving = true;
            EnemyData enemyData = new EnemyData();

            for (int i = 0; i < enemies.Length; i++)
            {
                enemyData.health[i] = enemies[i]._enemyStats.health;
                enemyData.alive[i] = enemies[i].isAlive;
                enemyData.searching[i] = enemies[i].searching;
                enemyData.currentPatrolPoints[i] = enemies[i].currentPatrolPoint;
                enemyData.statesAsInt[i] = (int)enemies[i].enemyState;
                enemyData.patrolPointParentNames[i] = enemies[i].patrolPointParentName;
                enemyData.enemySavePos[i] = new SerializableVector3();
                enemyData.enemySavePos[i].SetSerializableVector(enemies[i].saveGamePos);
            }

            binaryFormatter.Serialize(enemyFile, enemyData);
            enemyFile.Close();
            saving = false;
        }

        public void LoadFromEnemyData()
        {
            if (File.Exists(Application.persistentDataPath + "/enemyData.dat"))
            {
                Debug.Log("File found, loading enemy data");
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream enemyFile = File.Open(Application.persistentDataPath + "/enemyData.dat", FileMode.Open);
                if (enemyFile.Length == 0)
                {
                    enemyFile.Close();
                    Debug.Log("File length 0, closing file");
                }
                else
                {
                    enemyFile.Position = 0;
                    EnemyData enemyData = (EnemyData)binaryFormatter.Deserialize(enemyFile);
                    enemyFile.Close();

                    for (int i = 0; i < enemies.Length; i++)
                    {
                        enemies[i]._enemyStats.health = enemyData.health[i];
                        enemies[i].isAlive = enemyData.alive[i];
                        enemies[i].searching = enemyData.searching[i];
                        enemies[i].currentPatrolPoint = enemyData.currentPatrolPoints[i];
                        enemies[i].enemyState = (Enemy.EnemyState)enemyData.statesAsInt[i];
                        enemies[i].patrolPointParentName = enemyData.patrolPointParentNames[i];

                        enemyData.enemySavePos[i].GetSerializableVector(ref enemies[i].saveGamePos);
                        //Debug.Log("Saved enemy pos: " + enemies[i].saveGamePos);
                        
                    }
                }
            }
            else
            {
                //set stats to default if save is missing or unreadable
                Debug.Log("File not found: enemyData.dat");
            }
        }

        public void EnemyManagerToDataManager()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = Enemy_Manager.enemy_Manager.enemies[i];
            }

            SaveToEnemyData();
        }

        public void DataManagerToEnemyManager()
        {
            
            for (int i = 0; i < enemies.Length; i++)
            {
                Enemy_Manager.enemy_Manager.enemies[i].transform.position = enemies[i].saveGamePos;
                Enemy_Manager.enemy_Manager.enemies[i].enemyState = enemies[i].enemyState;
                Enemy_Manager.enemy_Manager.enemies[i]._enemyStats.health = enemies[i]._enemyStats.health;
                Enemy_Manager.enemy_Manager.enemies[i].searching = enemies[i].searching;
                Enemy_Manager.enemy_Manager.enemies[i].patrolPointParentName = enemies[i].patrolPointParentName;
                Enemy_Manager.enemy_Manager.enemies[i].currentPatrolPoint = enemies[i].currentPatrolPoint;
                Enemy_Manager.enemy_Manager.enemies[i].isAlive = enemies[i].isAlive;

            }
        }

        // -------------------------------------------------------------------------------------------------------------------------------------------------- Interactable Data

        private void SaveToInteractableData()
        {
            saving = true;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream itemFile = File.Create(Application.persistentDataPath + "/itemData.dat");
            InteractableData itemData = new InteractableData();

            for (int i = 0; i < heldItems.Length; i++)
            {
                if (heldItems[i] == null) continue;
                if (heldItems[i].GetComponent<Interactable>() == null) continue;
                itemData.heldItems[i] = heldItems[i].name;
            }

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetComponent<Weapon>())
                {
                    itemData.isEquipped[i] = items[i].GetComponent<Weapon>().isEquiped;
                    itemData.currentAmmo[i] = items[i].GetComponent<Weapon>().currentAmmo;
                    itemData.reserves[i] = items[i].GetComponent<Weapon>().reserves;
                }
                else if (items[i].GetComponent<Item>())
                {
                    itemData.isEquipped[i] = items[i].GetComponent<Item>().isEquiped;
                    itemData.isUsed[i] = items[i].GetComponent<Item>().isUsed;
                }
                Debug.Log(i);
                itemData.itemSavePos[i] = new SerializableVector3();
                itemData.itemSavePos[i].SetSerializableVector(items[i].GetComponent<Interactable>().savePos);
                Debug.Log("Saved item pos: " + items[i].GetComponent<Interactable>().savePos);
            }

            binaryFormatter.Serialize(itemFile, itemData);
            itemFile.Close();
            saving = false;
        }

        public void LoadFromInteractableData()
        {
            if (File.Exists(Application.persistentDataPath + "/itemData.dat"))
            {
                Debug.Log("File found, loading item data");
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream itemFile = File.Open(Application.persistentDataPath + "/itemData.dat", FileMode.Open);
                if (itemFile.Length == 0)
                {
                    itemFile.Close();
                    Debug.Log("File length 0, closing file");
                }
                else
                {
                    itemFile.Position = 0;
                    InteractableData itemData = (InteractableData)binaryFormatter.Deserialize(itemFile);
                    itemFile.Close();

                    for (int i = 0; i < items.Length; i++)
                    {
                        if (items[i].GetComponent<Weapon>())
                        {
                            items[i].GetComponent<Weapon>().isEquiped = itemData.isEquipped[i];
                            items[i].GetComponent<Weapon>().currentAmmo = itemData.currentAmmo[i];
                            items[i].GetComponent<Weapon>().reserves = itemData.reserves[i];
                        }
                        else if (items[i].GetComponent<Item>())
                        {
                            items[i].GetComponent<Item>().isEquiped = itemData.isEquipped[i];
                            items[i].GetComponent<Item>().isUsed = itemData.isUsed[i];
                        }

                        itemData.itemSavePos[i].GetSerializableVector(ref items[i].GetComponent<Interactable>().savePos);
                        //Debug.Log("Saved item pos: " + items[i].GetComponent<Interactable>().savePos);
                    }

                    for (int i = 0; i < heldItems.Length; i++)
                    {
                        if (itemData.heldItems[i] == null) continue;
                        for (int j = 0; j < items.Length; j++)
                        {
                            
                            if (itemData.heldItems[i] == items[j].name)
                            heldItems[i] = items[j];
                        }  
                    }
                }
            }
            else
            {
                //set stats to default if save is missing or unreadable
                Debug.Log("File not found: itemData.dat");
            }

            DataManagerToInteractableManager();
        }

        public void InteractableManagerToDataManager()
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = Interactable_Manager.interactable_manager.interactables[i].gameObject;
            }
            InventoryToDataManager();
            SaveToInteractableData();
        }

        public void DataManagerToInteractableManager()
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetComponent<Weapon>())
                {
                    Interactable_Manager.interactable_manager.interactables[i].GetComponent<Weapon>().isEquiped = items[i].GetComponent<Weapon>().isEquiped;
                    Interactable_Manager.interactable_manager.interactables[i].GetComponent<Weapon>().reserves = items[i].GetComponent<Weapon>().reserves;
                    Interactable_Manager.interactable_manager.interactables[i].GetComponent<Weapon>().currentAmmo = items[i].GetComponent<Weapon>().currentAmmo;
                }
                else if (items[i].GetComponent<Item>())
                {
                    Interactable_Manager.interactable_manager.interactables[i].GetComponent<Item>().isEquiped = items[i].GetComponent<Item>().isEquiped;
                    Interactable_Manager.interactable_manager.interactables[i].GetComponent<Item>().isUsed = items[i].GetComponent<Item>().isUsed;
                }

                Interactable_Manager.interactable_manager.interactables[i].savePos = items[i].GetComponent<Interactable>().savePos;
            }

            DataManagerToInventory();
        }

        public void InventoryToDataManager()
        {
            for (int i = 0; i < heldItems.Length; i++)
            {
                if (PlayerInventory.playerInventory.inventory[i] == null) continue;
                heldItems[i] = PlayerInventory.playerInventory.inventory[i];
            }
        }

        public void DataManagerToInventory()
        {
            for (int i = 0; i < heldItems.Length; i++)
            {
                //Debug.Log(i);
                if (heldItems[i] == null)
                {
                    Debug.LogWarning("heldItems " + i + " is null");
                    continue;
                }
                if (heldItems[i].GetComponent<Interactable>() == null)
                {
                    Debug.LogWarning("not an interactable" + i);
                    continue;
                }
                for (int j = 0; j < items.Length; j++)
                {
                    if (heldItems[i].name != Interactable_Manager.interactable_manager.interactables[j].name)
                    {
                        continue;
                    }
                    Debug.Log("Setting player inventory slot");
                    Debug.Log(heldItems[i].name + " " + Interactable_Manager.interactable_manager.interactables[j].name);
                    //PlayerInventory.playerInventory.inventory[i] = Interactable_Manager.interactable_manager.interactables[j].gameObject;
                    PlayerInventory.playerInventory.AddToInventory(Interactable_Manager.interactable_manager.interactables[j].gameObject);
                    Debug.Log(Interactable_Manager.interactable_manager.interactables[j].gameObject.transform.localPosition + " " + Interactable_Manager.interactable_manager.interactables[j].gameObject.transform.position + " " + Interactable_Manager.interactable_manager.interactables[j].gameObject.transform.parent.gameObject.name);
                    return;
                }
            }
        }

    }



    // ------------------------------------------------------------------------------------------------------------------------------------------------------ Serializable Classes

    //containers to store stats as files
    [Serializable]
    public class GlobalData
    {
        public float masterVolume;
        public float musicVolume;
        public float SFXVolume;
    }

    [Serializable]
    public class PlayerData
    {
        //Store objective completion and stats here
        //Data from playerStats
        public float maxHealth;
        public float health;
        public float suitPower;
        public float maxSuitPower;

        //Data from objectiveManager
        public bool[] objectives = new bool[Enum.GetNames(typeof(Objective_Manager.Objectives)).Length];
        public bool finalObjectiveComplete;
        public int numberOfObjectivesComplete;
        public int objectivesToWin;

        //Data from firstPersonController_Sam
        public bool inWater;
        public bool carryingHeavyObj;

        public SerializableVector3 playerSV = new SerializableVector3();
        public SerializableQuaternion playerSQ = new SerializableQuaternion();
    }

    [Serializable]
    public class EnemyData
    {
        public int[] statesAsInt = new int[Data_Manager.dataManager.numberOfEnemies];

        public SerializableVector3[] enemySavePos = new SerializableVector3[Data_Manager.dataManager.numberOfEnemies];
        public int[] health = new int[Data_Manager.dataManager.numberOfEnemies];
        public bool[] searching = new bool[Data_Manager.dataManager.numberOfEnemies];
        public string[] patrolPointParentNames = new string[Data_Manager.dataManager.numberOfEnemies];
        public int[] currentPatrolPoints = new int[Data_Manager.dataManager.numberOfEnemies];
        public bool[] alive = new bool[Data_Manager.dataManager.numberOfEnemies];


    }

    [Serializable]
    public class InteractableData
    {
        public SerializableVector3[] itemSavePos = new SerializableVector3[Data_Manager.dataManager.numberOfItems];

        public bool[] isEquipped = new bool[Data_Manager.dataManager.numberOfItems];
        public bool[] isUsed = new bool[Data_Manager.dataManager.numberOfItems];
        public int[] currentAmmo = new int[Data_Manager.dataManager.numberOfItems];
        public int[] reserves = new int[Data_Manager.dataManager.numberOfItems];
        public string[] heldItems = new string[3];
        
    }

}
