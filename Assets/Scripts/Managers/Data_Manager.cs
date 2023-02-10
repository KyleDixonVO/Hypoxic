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
        public readonly int numberOfObjectives = 4;

        //firstPersonControllerSam data
        public bool inWater;
        public bool carryingHeavyObj;

        //vector3 floats
        public float playerPosX;
        public float playerPosY;
        public float playerPosZ;

        //quaternion floats
        public float playerRotW;
        public float playerRotX;
        public float playerRotY;
        public float playerRotZ;

        //enemyManager
        public readonly int numberOfEnemies = 5;
        Enemy[] enemies;




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
            objectives = new bool[numberOfObjectives];
            enemies = new Enemy[numberOfEnemies];
        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool SaveExists()
        {
            if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
            {
                return true;
            }
            return false;
        }

        //saves out settings prefs
        public void SaveGlobalData()
        {
            Debug.Log("Saving Global Data");
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream globalFile = File.Create(Application.persistentDataPath + "/globalData.dat");
            saving = true;
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
                Debug.Log("Loading Global Data");
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
                ResetGlobalPrefs();
            }
        }

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
                    objectives[(int)Objective_Manager.Objectives.repairFirstPipe] = playerData.objectives[(int)Objective_Manager.Objectives.repairFirstPipe];
                    objectives[(int)Objective_Manager.Objectives.repairSecondPipe] = playerData.objectives[(int)Objective_Manager.Objectives.repairSecondPipe];
                    objectives[(int)Objective_Manager.Objectives.repairThirdPipe] = playerData.objectives[(int)Objective_Manager.Objectives.repairThirdPipe];
                    objectives[(int)Objective_Manager.Objectives.goToElevator] = playerData.objectives[(int)Objective_Manager.Objectives.goToElevator];
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
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream playerFile = File.Create(Application.persistentDataPath + "/playerData.dat");
            saving = true;
            PlayerData playerData = new PlayerData();

            playerData.maxSuitPower = maxSuitPower;
            playerData.suitPower = suitPower;
            playerData.maxHealth = maxPlayerHealth;
            playerData.health = playerHealth;
            playerData.objectives[(int)Objective_Manager.Objectives.repairFirstPipe] = objectives[(int)Objective_Manager.Objectives.repairFirstPipe];
            playerData.objectives[(int)Objective_Manager.Objectives.repairSecondPipe] = objectives[(int)Objective_Manager.Objectives.repairSecondPipe];
            playerData.objectives[(int)Objective_Manager.Objectives.repairThirdPipe] = objectives[(int)Objective_Manager.Objectives.repairThirdPipe];
            playerData.objectives[(int)Objective_Manager.Objectives.goToElevator] = objectives[(int)Objective_Manager.Objectives.goToElevator];
            playerData.inWater = inWater;
            playerData.carryingHeavyObj = carryingHeavyObj;
            RotPosToPlayerData(playerData);

            binaryFormatter.Serialize(playerFile, playerData);
            playerFile.Close();
            saving = false;
        }

        //saves to Data_Manager from source classes
        public void SaveToDataManager()
        {
            objectives[(int)Objective_Manager.Objectives.repairFirstPipe] = Objective_Manager.objective_Manager.GetObjectiveState(Objective_Manager.Objectives.repairFirstPipe);
            objectives[(int)Objective_Manager.Objectives.repairSecondPipe] = Objective_Manager.objective_Manager.GetObjectiveState(Objective_Manager.Objectives.repairSecondPipe);
            objectives[(int)Objective_Manager.Objectives.repairThirdPipe] = Objective_Manager.objective_Manager.GetObjectiveState(Objective_Manager.Objectives.repairThirdPipe);
            objectives[(int)Objective_Manager.Objectives.goToElevator] = Objective_Manager.objective_Manager.GetObjectiveState(Objective_Manager.Objectives.goToElevator);
            if (PlayerStats.playerStats == null) return;
            maxSuitPower = PlayerStats.playerStats.maxSuitPower;
            suitPower = PlayerStats.playerStats.suitPower;
            maxPlayerHealth = PlayerStats.playerStats.maxPlayerHealth;
            playerHealth = PlayerStats.playerStats.playerHealth;
            inWater = FirstPersonController_Sam.fpsSam.inWater;
            carryingHeavyObj = FirstPersonController_Sam.fpsSam.carryingHeavyObj;
            SavedPosToFloats();
            SavedRotToFloats();
            SaveToPlayerData();
        }

        //converts vector3 to floats for serializing
        public void SavedPosToFloats()
        {
            playerPosX = FirstPersonController_Sam.fpsSam.playerSavedPosition.x;
            playerPosY = FirstPersonController_Sam.fpsSam.playerSavedPosition.y;
            playerPosZ = FirstPersonController_Sam.fpsSam.playerSavedPosition.z;
        }

        //converts floats back to vector3 to pass into fpsSam
        public void FloatsToSavedPos()
        {
            FirstPersonController_Sam.fpsSam.playerSavedPosition = new Vector3(playerPosX, playerPosY, playerPosZ);
        }

        //converts vector4 to floats for serializing
        public void SavedRotToFloats()
        {
            playerRotW = FirstPersonController_Sam.fpsSam.playerSavedRotation.w;
            playerRotX = FirstPersonController_Sam.fpsSam.playerSavedRotation.x;
            playerRotY = FirstPersonController_Sam.fpsSam.playerSavedRotation.y;
            playerRotZ = FirstPersonController_Sam.fpsSam.playerSavedRotation.z;
        }

        //converts floats back to vector4 to pass into fpsSam
        public void FloatsToSavedRot()
        {
            FirstPersonController_Sam.fpsSam.playerSavedRotation = new Quaternion(playerRotW, playerRotY, playerRotZ, playerRotW);
        }

        //passing floats from Data_Manager to playerData
        public void RotPosToPlayerData(PlayerData playerData)
        {
            playerData.playerRotW = playerRotW;
            playerData.playerRotX = playerRotX;
            playerData.playerRotY = playerRotY;
            playerData.playerRotZ = playerRotZ;

            playerData.playerPosX = playerPosX;
            playerData.playerPosY = playerPosY;
            playerData.playerPosZ = playerPosZ;
        }

        //passing floats from playerData to Data_Manager
        public void RotPosToManager(PlayerData playerData)
        {
            playerRotW = playerData.playerRotW;
            playerRotX = playerData.playerRotX;
            playerRotY = playerData.playerRotY;
            playerRotZ = playerData.playerRotZ;

            playerPosX = playerData.playerPosX;
            playerPosY = playerData.playerPosY;
            playerPosZ = playerData.playerPosZ;
        }

        public void ResetPlayerData()
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
            objectives[(int)Objective_Manager.Objectives.repairFirstPipe] = false;
            objectives[(int)Objective_Manager.Objectives.repairSecondPipe] = false;
            objectives[(int)Objective_Manager.Objectives.repairThirdPipe] = false;
            objectives[(int)Objective_Manager.Objectives.goToElevator] = false;

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
            FloatsToSavedRot();
        }

        public void ResetGlobalPrefs()
        {
            mastervolume = 1.0f;
            musicVolume = 1.0f;
            SFXVolume = 1.0f;
        }

        public void SaveToEnemyData()
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
                //enemyData.patrolPointParentNames[i] = enemies[i].patrolPointParentName;
                enemyData.savePosX[i] = enemies[i].SaveGamePos.x;
                enemyData.savePosY[i] = enemies[i].SaveGamePos.y;
                enemyData.savePosZ[i] = enemies[i].SaveGamePos.z;
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
                        //enemies[i].patrolPointParentName = enemyData.patrolPointParentNames[i];
                        enemies[i].SaveGamePos.x = enemyData.savePosX[i];
                        enemies[i].SaveGamePos.y = enemyData.savePosY[i];
                        enemies[i].SaveGamePos.z = enemyData.savePosZ[i];
                    }
                }
            }
            else
            {
                //set stats to default if save is missing or unreadable
                Debug.Log("File not found, resetting player stats to default");
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
                Enemy_Manager.enemy_Manager.enemies[i] = enemies[i];
            }
        }
    }


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
        public bool[] objectives = new bool[Data_Manager.dataManager.numberOfObjectives];
        public bool finalObjectiveComplete;
        public int numberOfObjectivesComplete;
        public int objectivesToWin;

        //Data from firstPersonController_Sam
        public bool inWater;
        public bool carryingHeavyObj;

        public float playerPosX;
        public float playerPosY;
        public float playerPosZ;

        public float playerRotW;
        public float playerRotX;
        public float playerRotY;
        public float playerRotZ;
    }

    [Serializable]
    public class EnemyData
    {
        public int[] statesAsInt = new int[Data_Manager.dataManager.numberOfEnemies];
        public float[] savePosX = new float[Data_Manager.dataManager.numberOfEnemies];
        public float[] savePosY = new float[Data_Manager.dataManager.numberOfEnemies];
        public float[] savePosZ = new float[Data_Manager.dataManager.numberOfEnemies];
        public int[] health = new int[Data_Manager.dataManager.numberOfEnemies];
        public bool[] searching = new bool[Data_Manager.dataManager.numberOfEnemies];
        public string[] patrolPointParentNames = new string[Data_Manager.dataManager.numberOfEnemies];
        public int[] currentPatrolPoints = new int[Data_Manager.dataManager.numberOfEnemies];
        public bool[] alive = new bool[Data_Manager.dataManager.numberOfEnemies];


    }
}
