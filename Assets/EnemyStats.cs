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
    public float searchingSpeed;
    public float startSearchingTime;
    [HideInInspector] public float searchingTime;

    [Header("AttackingSettings")]
    public float attackSpeed;

    [Header("DyingSettings")]
    public float dyingTime;
}
