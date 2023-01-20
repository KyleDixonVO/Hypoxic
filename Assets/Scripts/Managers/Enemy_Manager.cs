using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnderwaterHorror;

public class Enemy_Manager : MonoBehaviour
{
    public static Enemy_Manager enemy_Manager;
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
