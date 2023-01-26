using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    //Code by Tobias
    public class BigEnemyFish : Enemy
    {
        // Start is called before the first frame update

        override protected void DefeatedManager()
        {
            // Fleeing Time
            _enemyStats.fleeingTime -= Time.deltaTime;

            agent.speed = _enemyStats.fleeingSpeed;
            agent.SetDestination(FirstPersonController_Sam.fpsSam.transform.position - transform.position);

            // Enemy may despawn after fleeing in the future

            if (_enemyStats.fleeingTime <= 0)
            {
                enemyState = EnemyState.patrolling;
                _enemyStats.health = _enemyStats.maxHealth;
                _enemyStats.elapsedFleeingTime = _enemyStats.fleeingTime;
            }
        }

    }
}
