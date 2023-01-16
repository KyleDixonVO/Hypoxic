using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float startSearchingTime;
    [HideInInspector] public float searchingTime;

    [Header("AttackingSettings")]
    public float attackSpeed;

    [Header("DefeatedSettings")]
    // BigEnemyFish not effected by dying time/SmallEnemyFish not effected by fleeing time.
    [Header("BigEnemyFish")]
    public float fleeingSpeed;
    public float fleeingTime;
    [Header("SmallEnemyFish")]
    public float dyingTime;
}
