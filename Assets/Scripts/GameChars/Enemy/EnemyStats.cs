using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    //Code by Tobias
    public class EnemyStats : MonoBehaviour
    {
        [Header("HealthSettings")]
        public int health = 5;
        [Header("PatrolSettings")]
        public float patrolSpeed;
        public int patrolRandomWaitTimerWeight;
        [HideInInspector] public float patrolRandomWaitTimer;

        [Header("ChasingSettings")]
        public float chaseSpeed;

        [Header("SearchingSettings")]
        public float searchingMovementSpeed;
        public float searchingTimeStart;
        [HideInInspector] public float searchingTime;

        [Header("AttackingSettings")]
        public float attackPower;
        public float timeToAttackStart;
        public float timeToAttack;

        [Header("DefeatedSettings")]
        // BigEnemyFish not effected by dying time/SmallEnemyFish not effected by fleeing time.
        [Header("BigEnemyFish")]
        public float fleeingSpeed;
        public float fleeingTime;
        [Header("SmallEnemyFish")]
        public float dyingTime;

        [Header("Detection distances")]
        //the actual range of the attack
        public float attackReach = 1;
        //how close can the enemy be before switching attack state
        public float attackStateRadius = 4;
        public float runningRadius = 14;
        public float walkingRadius = 8;
        public float crounchingRadius = 5;
        public float visionRayLength = 10;

        public void TakeDamage(int playerAttack)
        {
            health -= playerAttack;

            if (health <= 0)
            {
                health = 0;          
            }
        }

        public void ResetRun()
        {
            health = 5;
        }

        public float GetDetectionDistance()
        {
            if (FirstPersonController_Sam.fpsSam.IsRunning())
            {
                return runningRadius;
            }
            else if (FirstPersonController_Sam.fpsSam.IsCrouching())
            {
                return crounchingRadius;
            }
            else
            {
                return walkingRadius;
            }
        }
    }

}
