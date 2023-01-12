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
    public float mastervolume;
    public float musicVolume;
    public float SFXVolume;

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

        //set stored values equal to current upgrade tiers

        binaryFormatter.Serialize(playerFile, playerData);
        playerFile.Close();
        saving = false;
    }

    public void ResetPlayerData()
    {
        //reset player stats to default and then save stats

        //place default references here

        SavePlayerData();
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
}
