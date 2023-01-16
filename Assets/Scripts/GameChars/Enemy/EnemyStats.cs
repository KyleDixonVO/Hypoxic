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
        [HideInInspector] public float timeToAttack;

        [Header("DefeatedSettings")]
        // BigEnemyFish not effected by dying time/SmallEnemyFish not effected by fleeing time.
        [Header("BigEnemyFish")]
        public float fleeingSpeed;
        public float fleeingTime;
        [Header("SmallEnemyFish")]
        public float dyingTime;
    }

}
