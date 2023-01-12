using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UnderwaterHorror
{
    public class Enemy : EnemyStats
    {
        public bool isAlive;
        protected enum EnemyState
        {
            patrolling,
            alerted,
            chasing,
            attacking,
            searching,
            fleeing,
            dying
        }

        protected EnemyState enemyState;

        [Header("Scripts")]
        [SerializeField] protected EnemyFOV _enemyFOV;

        [Header("GameObjects")]
        [SerializeField] protected GameObject playerObj;
        
        [Header("NavMesh")]
        [SerializeField] protected NavMeshAgent agent;

        [Header("FOVRaycast")]
        [SerializeField] protected LayerMask layerMasks;

        [Header("PatrolSettings")]
        [SerializeField] protected float patrolSpeed;
        [SerializeField] protected BoxCollider patrolCollider;
        [SerializeField] protected List<GameObject> patrolPoints = new List<GameObject>();
        [Header("Place desired patrol point here")]
        [SerializeField] protected GameObject currentPatrolPoint;
        [SerializeField] protected int patrolRandomWaitTimerWeight;
        protected float patrolRandomWaitTimer;

        [Header("ChasingSettings")]
        [SerializeField] protected float chaseSpeed;
        [SerializeField] protected SphereCollider chaseCollider;

        [Header("SearchingSettings")]
        [SerializeField] protected float searchingSpeed;
        [SerializeField] protected float startSearchingTime;
        protected float searchingTime;

        [Header("AttackingSettings")]
        [SerializeField] protected float attackSpeed;

        protected Vector3 playerPreviousLocation;

        // Update is called once per frame
        protected void Update()
        {
            switch (enemyState)
            {
                case EnemyState.patrolling:
                    PatrollingManager();
                    if (SpottedPlayer())
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
                    ChasingManager();
                    if (SpottedPlayer() == false)
                    {
                        enemyState = EnemyState.searching;
                    }
                    break;

                case EnemyState.attacking:
                    // attacks player when in range
                    
            
                case EnemyState.searching:
                    // Searches where player was last seen
                    SearchingManager();
                    if (SpottedPlayer() == true)
                    {
                        enemyState = EnemyState.chasing;
                    }
                    break;

                case EnemyState.fleeing:
                    // Enemy flees for determined time

                    break;

                case EnemyState.dying:
                    // Starts dying animation
                    agent.isStopped = true;
                    break;
            }
        }

        void PatrollingManager()
        {
            agent.speed = patrolSpeed;
            // Follow patrol points in random order
            agent.SetDestination(currentPatrolPoint.transform.position);

            float distCheck = Vector3.Distance(currentPatrolPoint.transform.position, this.gameObject.transform.position);

            if (distCheck < 0.5)
            {
                if (patrolRandomWaitTimer <= 0)
                {
                    int randomPoint = Random.Range(0, patrolPoints.Count);
                    currentPatrolPoint = patrolPoints[randomPoint];
                    Debug.Log("point: " + randomPoint);
                }
                else
                {
                    patrolRandomWaitTimer -= Time.deltaTime;
                }
            }
            else
            {
                patrolRandomWaitTimer = Random.Range(0, patrolRandomWaitTimerWeight);
            }

            // POV Colliders
            patrolCollider.enabled = true;
            chaseCollider.enabled = false;
        }

        void ChasingManager()
        {
            agent.speed = chaseSpeed;
            // Chases after the players position
            agent.SetDestination(playerObj.transform.position);

            // Tracks player's previous location
            playerPreviousLocation = playerObj.transform.position;

            patrolCollider.enabled = false;
            chaseCollider.enabled = true;
        }

        void SearchingManager()
        {
            agent.speed = searchingSpeed;
            // Goes to last spot the player was before searching

            float distCheck = Vector3.Distance(playerPreviousLocation, this.gameObject.transform.position);

            if (distCheck < 0.5)
            {
                agent.SetDestination(playerPreviousLocation);
                searchingTime = startSearchingTime;
            }

            else if (distCheck > 0.5)
            {
                // Searching Movement...
                searchingTime -= Time.time;

                if (searchingTime <= 0)
                {
                    enemyState = EnemyState.patrolling;
                }
            }

            // POV Colliders
            patrolCollider.enabled = true;
            chaseCollider.enabled = false;
        }

        protected bool SpottedPlayer()
        {
            // If sight touches player
            if (_enemyFOV.playerInFOV)
            {
                Debug.Log("InsidePOV");
                Vector3 raycastDir = playerObj.transform.position;
                if (!Physics.Linecast(this.gameObject.transform.position, raycastDir, layerMasks))
                {
                    Debug.Log("NotBlocked");
                    return true;
                }

                else
                {
                    Debug.Log("Blocked");
                    return false;
                }
            }
            return false;
        }

        protected bool LostPlayer()
        {
            // If player leaves sight
            return false;
        }
    }
}
