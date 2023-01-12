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

        [Header("Lists")]
        [SerializeField] protected List<GameObject> patrolPoints = new List<GameObject>();

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
                    // Patrols to determined points randomly
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
                    // Chases after player
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
