using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UnderwaterHorror
{
    public class Enemy : MonoBehaviour
    {
        protected enum EnemyState
        {
            patrolling,
            alerted,
            chasing,
            attacking,
            searching,
            dying
        }

        protected EnemyState enemyState;

        [Header("GameObjects")]
        [SerializeField] protected GameObject playerObj;

        [Header("PatrolSettings")]
        [SerializeField] protected List<GameObject> patrolPoints = new List<GameObject>();
        [Header("Place desired patrol point here")]
        [SerializeField] protected GameObject currentPatrolPoint;

        [Header("NavMesh")]
        [SerializeField] protected NavMeshAgent agent;

        [Header("Stats")]
        [SerializeField] protected float patrolSpeed;
        [SerializeField] protected float chaseSpeed;
        [SerializeField] protected float attackSpeed;
        [SerializeField] protected float searchingSpeed;


        // Update is called once per frame
        protected void Update()
        {
            switch (enemyState)
            {
                case EnemyState.patrolling:
                    PatrolManager();
                    if (SpottedPlayer() == true)
                    {
                        enemyState = EnemyState.chasing;
                    }
                    break;

                case EnemyState.alerted:
                    // Looks Direction it was alerted to after a delay
                    if (SpottedPlayer() == true)
                    {
                        enemyState = EnemyState.chasing;
                    }
                    break;

                case EnemyState.chasing:
                    ChaseManager();
                    if (LostPlayer() == true)
                    {
                        enemyState = EnemyState.searching;
                    }
                    break;

                case EnemyState.attacking:
                    // attacks player when in range
                
            
                case EnemyState.searching:
                    // Searches where player was last seen
                    if (SpottedPlayer() == true)
                    {
                        enemyState = EnemyState.chasing;
                    }
                    break;

                case EnemyState.dying:
                    // Starts dying animation
                    break;
            }
        }

        void PatrolManager()
        {
            agent.speed = patrolSpeed;
            // Follow patrol points in random order
            agent.SetDestination(currentPatrolPoint.transform.position);

            float distCheck = Vector3.Distance(currentPatrolPoint.transform.position, this.gameObject.transform.position);

            if (distCheck < 0.5)
            {
                int randomPoint = Random.Range(0, patrolPoints.Count);
                currentPatrolPoint = patrolPoints[randomPoint];
                Debug.Log("point: " + randomPoint);
            }
        }

        void ChaseManager()
        {
            agent.speed = chaseSpeed;
            // Chases after the players position
            agent.SetDestination(playerObj.transform.position);
        }

        protected bool SpottedPlayer()
        {
            // If sight touches player
            return false;
        }

        protected bool LostPlayer()
        {
            // If player leaves sight
            return false;
        }
    }
}
