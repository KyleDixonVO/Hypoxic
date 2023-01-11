using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum EnemyState
    {
        patroling,
        chasing,
        searching,
    }

    EnemyState enemyState;
    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.patroling;
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.patroling:
                // Patrols to determined points randomly
                if (SpottedPlayer() == true)
                {
                    enemyState = EnemyState.chasing;
                }
                break;
            
            case EnemyState.chasing:  
                // Chases after player
                if (LostPlayer() == true)
                {
                    enemyState = EnemyState.searching;
                }
                break;
            
            case EnemyState.searching:
                // Searches where player was last seen
                if (SpottedPlayer() == true)
                {
                    enemyState = EnemyState.chasing;
                }
                break;
        }
    }

    bool SpottedPlayer()
    {
        // If sight touches player
        return false;
    }

    bool LostPlayer()
    {
        // If player leaves sight
        return false;
    }

    bool Alerted()
    {
        // If heard a sound or spotted the flashlight
        return false;
    }
}
