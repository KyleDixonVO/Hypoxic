using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnderwaterHorror;

public class Enemy_Manager : MonoBehaviour
{
    public static Enemy_Manager enemy_Manager;
    public Enemy[] enemies;
    private int numberOfEnemies = 5;

    private void Awake()
    {
        if (enemy_Manager == null)
        {
            enemy_Manager = this;
            DontDestroyOnLoad(this);
        }
        else if (enemy_Manager != null && enemy_Manager != this)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemies = new Enemy[numberOfEnemies];
    }

    // Update is called once per frame
    void Update()
    {
        EnemySingleton();
    }

    void EnemySingleton()
    {
        if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
        for (int i = 0; i < enemies.Length; i++)
        {

            switch (i)
            {
                case 0:
                    if (enemies[i] == null)
                    {
                        enemies[i] = GameObject.Find("BigEnemy").GetComponent<Enemy>();
                        DontDestroyOnLoad(enemies[i]);
                    }
                    else if (enemies[i] != null && enemies[i] != GameObject.Find("BigEnemy").GetComponent<Enemy>())
                    {
                        Destroy(GameObject.Find("BigEnemy"));
                    }
                    break;

                case 1:
                    if (enemies[i] == null)
                    {
                        enemies[i] = GameObject.Find("SmallEnemy1").GetComponent<Enemy>();
                        DontDestroyOnLoad(enemies[i]);
                    }
                    else if (enemies[i] != null && enemies[i] != GameObject.Find("SmallEnemy1").GetComponent<Enemy>())
                    {
                        Destroy(GameObject.Find("SmallEnemy1"));
                    }
                    break;

                case 2:
                    if (enemies[i] == null)
                    {
                        enemies[i] = GameObject.Find("SmallEnemy2").GetComponent<Enemy>();
                        DontDestroyOnLoad(enemies[i]);
                    }
                    else if (enemies[i] != null && enemies[i] != GameObject.Find("SmallEnemy2").GetComponent<Enemy>())
                    {
                        Destroy(GameObject.Find("SmallEnemy2"));
                    }
                    break;

                case 3:
                    if (enemies[i] == null)
                    {
                        enemies[i] = GameObject.Find("SmallEnemy3").GetComponent<Enemy>();
                        DontDestroyOnLoad(enemies[i]);
                    }
                    else if (enemies[i] != null && enemies[i] != GameObject.Find("SmallEnemy3").GetComponent<Enemy>())
                    {
                        Destroy(GameObject.Find("SmallEnemy3"));
                    }
                    break;

                case 4:
                    if (enemies[i] == null)
                    {
                        enemies[i] = GameObject.Find("SmallEnemy4").GetComponent<Enemy>();
                        DontDestroyOnLoad(enemies[i]);
                    }
                    else if (enemies[i] != null && enemies[i] != GameObject.Find("SmallEnemy4").GetComponent<Enemy>())
                    {
                        Destroy(GameObject.Find("SmallEnemy4"));
                    }
                    break;

                //case 5:
                //    if (enemies[i] == null)
                //    {
                //        enemies[i] = GameObject.Find("SmallEnemy5").GetComponent<Enemy>();
                //        DontDestroyOnLoad(enemies[i]);
                //    }
                //    else if (enemies[i] != null && enemies[i] != GameObject.Find("SmallEnemy5").GetComponent<Enemy>())
                //    {
                //        Destroy(GameObject.Find("SmallEnemy5"));
                //    }
                //    break;
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
    }

    public void ResetToLastSave()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].ReloadToSave();
        }
    }

    public void LoadEnemyStates()
    {
        Data_Manager.dataManager.DataManagerToEnemyManager();
    }
}
