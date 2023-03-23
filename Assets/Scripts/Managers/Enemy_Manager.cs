using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnderwaterHorror;

public class Enemy_Manager : MonoBehaviour
{
    public static Enemy_Manager enemy_Manager;
    public Enemy[] enemies;
    public string[] enemyNames;
    private int numberOfEnemies;
    [SerializeField] private bool dataLoaded;
    public bool shouldLoad = false;

    private void Awake()
    {
        if (enemy_Manager == null)
        {
            enemy_Manager = this;
            DontDestroyOnLoad(this);
        }
        else if (enemy_Manager != null && enemy_Manager != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        dataLoaded = false;
        numberOfEnemies = enemyNames.Length;
        enemies = new Enemy[numberOfEnemies];
        //Debug.LogError(enemies.Length);
    }

    // Update is called once per frame
    void Update()
    {
        EnemySingleton();
        if (shouldLoad)
        {
            LoadEnemyStates();
        }
    }

    void EnemySingleton()
    {
        if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null && GameObject.Find(enemyNames[i]) != null)
            {
                enemies[i] = GameObject.Find(enemyNames[i]).GetComponent<Enemy>();
                enemies[i].singleton = true;
                DontDestroyOnLoad(enemies[i]);
            }
        }

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Enemy").Length; i++)
        {
            if (!GameObject.FindGameObjectsWithTag("Enemy")[i].GetComponent<Enemy>().singleton)
            {
                Destroy(GameObject.FindGameObjectsWithTag("Enemy")[i]);
            }
        }
    }

    public void ResetForNewRun()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null) return;
            enemies[i].ResetRun();
        }
    }

    public void SetSavePositions()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetSaveGamePos();
        }

        Data_Manager.dataManager.EnemyManagerToDataManager();
    }

    public void ResetToLastSave()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].ReloadToSave();
        }
    }

    public void SetShouldLoadTrue()
    {
        shouldLoad = true;
    }

    public void LoadEnemyStates()
    {
        for (int i = 0; i < enemies.Length; i++) if (enemies[i] == null) return;
        if (dataLoaded) return;
        Data_Manager.dataManager.DataManagerToEnemyManager();
        dataLoaded = true;
    }
}
