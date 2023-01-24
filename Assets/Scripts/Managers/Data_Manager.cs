using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

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
    public Vector3 playerPos;
    public Quaternion playerRot;



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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //saves out volume prefs
    public void SaveGlobalData()
    {
        Debug.Log("Saving Global Data");
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream globalFile = File.Create(Application.persistentDataPath + "/globalData.dat");
        saving = true;
        GlobalData globalData = new GlobalData();

        globalData.masterVolume = mastervolume;
        globalData.musicVolume = musicVolume;
        globalData.SFXVolume = SFXVolume;
        //Debug.Log(musicVolume + "  " + SFXVolume + " " + mastervolume);


        binaryFormatter.Serialize(globalFile, globalData);
        globalFile.Close();
        saving = false;
    }

    //loads volume prefs
    public void LoadGlobalData()
    {
        if (File.Exists(Application.persistentDataPath + "/globalData.dat"))
        {
            Debug.Log("Loading Global Data");
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream globalFile = File.Open(Application.persistentDataPath + "/globalData.dat", FileMode.Open);
            GlobalData globalData = (GlobalData)binaryFormatter.Deserialize(globalFile);
            globalFile.Close();

            mastervolume = globalData.masterVolume;
            musicVolume = globalData.musicVolume;
            SFXVolume = globalData.SFXVolume;
            //Debug.Log(musicVolume + "  " + SFXVolume + " " + mastervolume);
        }
        else
        {
            ResetGlobalPrefs();
        }
    }

    public void LoadPlayerData()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream playerFile = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);
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
            playerPos = playerData.playerPos;
            playerRot = playerData.playerRot;
        }
        else
        {
            //set stats to default if save is missing or unreadable
            ResetPlayerData();
        }
    }

    public void SavePlayerData()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream playerFile = File.Create(Application.persistentDataPath + "/playerData.dat");
        saving = true;
        PlayerData playerData = new PlayerData();

        //set stored values equal to current player stats
        playerData.maxSuitPower = PlayerStats.playerStats.maxSuitPower;
        playerData.suitPower = PlayerStats.playerStats.suitPower;
        playerData.maxHealth = PlayerStats.playerStats.maxPlayerHealth;
        playerData.health = PlayerStats.playerStats.playerHealth;
        playerData.objectives[(int)Objective_Manager.Objectives.repairFirstPipe] = Objective_Manager.objective_Manager.GetObjectiveState(Objective_Manager.Objectives.repairFirstPipe);
        playerData.objectives[(int)Objective_Manager.Objectives.repairSecondPipe] = Objective_Manager.objective_Manager.GetObjectiveState(Objective_Manager.Objectives.repairSecondPipe);
        playerData.objectives[(int)Objective_Manager.Objectives.repairThirdPipe] = Objective_Manager.objective_Manager.GetObjectiveState(Objective_Manager.Objectives.repairThirdPipe);
        playerData.objectives[(int)Objective_Manager.Objectives.goToElevator] = Objective_Manager.objective_Manager.GetObjectiveState(Objective_Manager.Objectives.goToElevator);
        playerData.inWater = FirstPersonController_Sam.fpsSam.inWater;
        playerData.carryingHeavyObj = FirstPersonController_Sam.fpsSam.carryingHeavyObj;
        playerData.playerRot = FirstPersonController_Sam.fpsSam.playerSavedRotation;
        playerData.playerPos = FirstPersonController_Sam.fpsSam.playerSavedPosition;

        binaryFormatter.Serialize(playerFile, playerData);
        playerFile.Close();
        saving = false;
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

        //fpsSam data
        inWater = false;
        carryingHeavyObj = false;

        //objectiveManager data
        Objective_Manager.objective_Manager.ResetRun();

        SavePlayerData();
    }

    public void UpdatePlayerStats()
    {
        PlayerStats.playerStats.maxPlayerHealth = maxPlayerHealth;
        PlayerStats.playerStats.playerHealth = playerHealth;
        PlayerStats.playerStats.maxSuitPower = maxSuitPower;
        PlayerStats.playerStats.suitPower = suitPower;
    }

    public void UpdateObjectiveManager()
    {
        for (int i = 0; i < objectives.Length; i++)
        {
            if (!objectives[i]) return;
            Objective_Manager.objective_Manager.UpdateObjectiveCompletion(i);
        }
    }

    public void UpdateFPSSam()
    {
        FirstPersonController_Sam.fpsSam.inWater = inWater;
        FirstPersonController_Sam.fpsSam.carryingHeavyObj = carryingHeavyObj;
        FirstPersonController_Sam.fpsSam.playerSavedPosition = playerPos;
        FirstPersonController_Sam.fpsSam.playerSavedRotation = playerRot;
    }

    public void ResetGlobalPrefs()
    {
        mastervolume = 1.0f;
        musicVolume = 1.0f;
        SFXVolume = 1.0f;
    }

}



[Serializable]
public class GlobalData
{
    public float masterVolume;
    public float musicVolume;
    public float SFXVolume;
}


//containers to store stats as files
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
    public bool[] objectives;
    public bool finalObjectiveComplete;
    public int numberOfObjectivesComplete;
    public int objectivesToWin;

    //Data from firstPersonController_Sam
    public bool inWater;
    public bool carryingHeavyObj;
    public Vector3 playerPos;
    public Quaternion playerRot;
}
