using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UnderwaterHorror
{
    public class Enemy : MonoBehaviour
    {
        public bool isAlive;
        protected enum EnemyState
        {
            patrolling,
            alerted,
            chasing,
            attacking,
            searching,
            defeated
        }

        protected EnemyState enemyState;

        [Header("Scripts")]
        [SerializeField] protected EnemyFOV _enemyFOV;
        [SerializeField] protected EnemyStats _enemyStats;

        [Header("GameObjects")]
        [SerializeField] protected List<GameObject> patrolPoints = new List<GameObject>();
        [SerializeField] protected GameObject playerObj;

        [Header("Place desired patrol point here")]
        [SerializeField] protected GameObject currentPatrolPoint;
        
        [Header("NavMesh")]
        [SerializeField] protected NavMeshAgent agent;

        [Header("FOVRaycast")]
        [SerializeField] protected LayerMask layerMasks;

        [Header("Colliders")]
        [SerializeField] protected BoxCollider patrolCollider;
        [SerializeField] protected SphereCollider chaseCollider;

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
                    break;
                    
            
                case EnemyState.searching:
                    SearchingManager();
                    if (SpottedPlayer() == true)
                    {
                        enemyState = EnemyState.chasing;
                    }
                    break;

                case EnemyState.defeated:
                    // Starts dying animation
                    DefeatedManager();                    
                    break;
            }

            if (_enemyStats.health <= 0)
            {
                enemyState = EnemyState.defeated;
            }
        }

        void PatrollingManager()
        {
            agent.speed = _enemyStats.patrolSpeed;
            // Follow patrol points in random order
            agent.SetDestination(currentPatrolPoint.transform.position);
            
            Vector3 pointPos = currentPatrolPoint.transform.position;
            Vector3 agentPos = this.agent.transform.position;

            Vector3 pointPosDest = new Vector3(pointPos.x, 0, pointPos.z);
            Vector3 agentPosDest = new Vector3(agentPos.x, 0, agentPos.z);

            float distCheck = Vector3.Distance(pointPosDest, agentPosDest);
            Debug.Log(distCheck);

            if (distCheck < 0.5)
            {
                if (_enemyStats.patrolRandomWaitTimer <= 0)
                {
                    int randomPoint = Random.Range(0, patrolPoints.Count);
                    currentPatrolPoint = patrolPoints[randomPoint];
                }
                else
                {
                    _enemyStats.patrolRandomWaitTimer -= Time.deltaTime;
                }
            }
            else
            {
                _enemyStats.patrolRandomWaitTimer = Random.Range(0, _enemyStats.patrolRandomWaitTimerWeight);
            }

            // POV Colliders
            patrolCollider.enabled = true;
            chaseCollider.enabled = false;
        }

        void ChasingManager()
        {
            agent.speed = _enemyStats.chaseSpeed;
            // Chases after the players position
            agent.SetDestination(playerObj.transform.position);

            // Tracks player's previous location
            playerPreviousLocation = playerObj.transform.position;           
            //---------------------------------------------------------

            patrolCollider.enabled = false;
            chaseCollider.enabled = true;
        }

        void SearchingManager()
        {
            agent.speed = _enemyStats.searchingMovementSpeed;
            // Goes to last spot the player was before searching

            float distCheck = Vector3.Distance(playerPreviousLocation, this.agent.transform.position);
            agent.SetDestination(playerPreviousLocation);

            if (distCheck > 1)
            {                
                //Debug.Log(playerPreviousLocation);
                //Debug.Log(this.gameObject.transform.position);
                _enemyStats.searchingTime = _enemyStats.startSearchingTime;
            }

            else
            {
                // Searching Movement...
                _enemyStats.searchingTime -= Time.deltaTime;

                if (_enemyStats.searchingTime <= 0)
                {
                    enemyState = EnemyState.patrolling;
                }
            }

            // POV Colliders
            patrolCollider.enabled = true;
            chaseCollider.enabled = false;
        }

        // DefeatedManager will be differen't for the two enemies
        virtual protected void DefeatedManager()
        {
            // Death Time
            agent.isStopped = true;
            _enemyStats.dyingTime -= Time.deltaTime;
            isAlive = false;

            if (_enemyStats.dyingTime <= 0)
            {
                this.gameObject.SetActive(false);
            }
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
                    //Debug.Log("NotBlocked");
                    return true;
                }

                else
                {
                    //Debug.Log("Blocked");
                    return false;
                }
            }
            return false;
        }
    }
}
