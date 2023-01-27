using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    //Code by Tobias
    public class EnemyStats : MonoBehaviour
    {
        [Header("HealthSettings")]
        public int maxHealth = 5;
        public int health;
        [Header("PatrolSettings")]
        public float patrolRadius;
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

        [Header("AlertedSettings")]
        public float alertCheckingTime;
        public float elapsedAlertTime;

        [Header("BigEnemyFish")]
        public float fleeingSpeed;
        public float fleeingTime;
        public float elapsedFleeingTime;
        public float awareTime;
        public float elapsedAwareTime;

        [Header("SmallEnemyFish")]
        public float elapsedDyingTime;
        public float dyingTime;

        [Header("Detection distances")]
        //the actual range of the attack
        public float attackReach;
        //how close can the enemy be before switching attack state
        public float attackStateRadius;

        //how far away can the player be while still being detected
        //the player will alert enemies automatically while half of the radius distance away
        public float runningRadius;
        public float walkingRadius;
        public float crouchingRadius;

        public float visionRayLength;
        public float visionConeHalved;

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
            health = maxHealth;
        }

        public float GetDetectionDistance()
        {
            if (FirstPersonController_Sam.fpsSam.IsRunning())
            {
                return runningRadius;
            }
            else if (FirstPersonController_Sam.fpsSam.IsCrouching())
            {
                return crouchingRadius;
            }
            else
            {
                return walkingRadius;
            }
        }
    }

}
