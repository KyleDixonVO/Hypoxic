using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class BigEnemyFish : Enemy
    {
        // Start is called before the first frame update
        void Start()
        {
            isAlive = true;
            enemyState = EnemyState.patrolling;
        }

        override protected void DefeatedManager()
        {
            // Fleeing Time
            _enemyStats.fleeingTime -= Time.deltaTime;

            // Opposite Directron from player
            Vector3 oppositeDir = -playerObj.transform.position;
            agent.SetDestination(oppositeDir);

            // Enemy may despawn after fleeing in the future

            if (_enemyStats.fleeingTime <= 0)
            {
                enemyState = EnemyState.patrolling;
                // Magic number for max health
                _enemyStats.health = 1;

                // Magic number for max fleeingTime
                _enemyStats.fleeingTime = 2;
            }
        }
    }
}
