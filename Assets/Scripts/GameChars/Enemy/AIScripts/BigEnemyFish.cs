using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    //Code by Tobias
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

            agent.speed = _enemyStats.fleeingSpeed;
            agent.SetDestination(playerObj.transform.position - transform.position);

            // Enemy may despawn after fleeing in the future

            if (_enemyStats.fleeingTime <= 0)
            {
                enemyState = EnemyState.patrolling;
                // Magic number for max health
                _enemyStats.health = 1;

                // Magic number for max fleeingTime
                _enemyStats.fleeingTime = 6;
            }
        }
    }
}
