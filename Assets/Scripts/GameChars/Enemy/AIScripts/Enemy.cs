using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace UnderwaterHorror
{
    //Code by Tobias
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
        [SerializeField] protected EnemyAttackRadius _enemyAttackRadius;
        [SerializeField] protected EnemyStats _enemyStats;

        [Header("GameObjects")]
        [SerializeField] protected List<GameObject> patrolPoints = new List<GameObject>();
        [SerializeField] protected GameObject playerObj;
        [SerializeField] protected GameObject attackAnimation;

        [Header("Place desired patrol point here")]
        [SerializeField] protected GameObject currentPatrolPoint;
        
        [Header("NavMesh")]
        [SerializeField] protected NavMeshAgent agent;

        [Header("FOVRaycast")]
        [SerializeField] protected LayerMask layerMasks;

        [Header("Colliders")]
        [SerializeField] protected BoxCollider patrolCollider;
        [SerializeField] protected SphereCollider chaseCollider;

        [SerializeField] private Vector3 NewGamePos;
        [SerializeField] private Vector3 SaveGamePos;
        protected Vector3 playerPreviousLocation;

        // Update is called once per frame
        protected void Update()
        {
            // Made by Kyle
            //-----------------------------------
            FindPlayerRef();
            //-----------------------------------

            switch (enemyState)
            {
                //-----------------------------------------  
                case EnemyState.patrolling:
                    PatrollingManager();
                    if (SpottedPlayer())
                    {
                        enemyState = EnemyState.chasing;
                    }
                    break;
                //-----------------------------------------  
                case EnemyState.alerted:
                    // Looks Direction it was alerted to after a delay
                    if (SpottedPlayer() == true)
                    {
                        enemyState = EnemyState.chasing;
                    }
                    break;
                //-----------------------------------------  
                case EnemyState.chasing:
                    ChasingManager();
                    if (SpottedPlayer() == false)
                    {
                        enemyState = EnemyState.searching;
                    }                       
                    else if (CanAttackPlayer())
                    {
                        enemyState = EnemyState.attacking;
                    }
                    break;
                //-----------------------------------------  
                case EnemyState.attacking:
                    // Will still chase player
                    ChasingManager();
                    // attacks player when in range
                    AttackingManager();
                    if (CanAttackPlayer() == false)
                    {
                        enemyState = EnemyState.chasing;
                    }
                    break;                   
                //-----------------------------------------          
                case EnemyState.searching:
                    SearchingManager();
                    if (SpottedPlayer() == true)
                    {
                        enemyState = EnemyState.chasing;
                    }
                    break;
                //-----------------------------------------  
                case EnemyState.defeated:
                    // Starts dying animation
                    DefeatedManager();                    
                    break;
                //-----------------------------------------  
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
            
            // Fixes find Dest bug 
            Vector3 pointPos = currentPatrolPoint.transform.position;
            Vector3 agentPos = this.agent.transform.position;

            Vector3 pointPosDest = new Vector3(pointPos.x, 0, pointPos.z);
            Vector3 agentPosDest = new Vector3(agentPos.x, 0, agentPos.z);
            //--------------------------------------------------------------

            float distCheck = Vector3.Distance(pointPosDest, agentPosDest);
            //Debug.Log(distCheck);

            if (distCheck < 6)
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

        void AttackingManager()
        {
            _enemyStats.timeToAttack -= Time.deltaTime;
            if (_enemyStats.timeToAttack <= 0)
            {
                playerObj.GetComponent<PlayerStats>().TakeDamage(_enemyStats.attackPower);
                _enemyStats.timeToAttack = _enemyStats.timeToAttackStart;

                // Turns animation on
                //attackAnimation.SetActive(true);
            }

            else
            {
                // Turns animation off
                //attackAnimation.SetActive(false);
            }
        }

        void SearchingManager()
        {
            agent.speed = _enemyStats.searchingMovementSpeed;


            // Fixes find Dest bug 
            Vector3 previousPos = playerPreviousLocation;
            Vector3 agentPos = this.agent.transform.position;

            Vector3 previousPosDest = new Vector3(previousPos.x, 0, previousPos.z);
            Vector3 agentPosDest = new Vector3(agentPos.x, 0, agentPos.z);
            // ------------------------------------------------------------------------

            float distCheck = Vector3.Distance(previousPosDest, agentPosDest);

            // Goes to last spot the player was last scene
            agent.SetDestination(playerPreviousLocation); //<--- playerPreviousLocation gets set in ChasingManager 

            if (distCheck > 1)
            {                
                //Debug.Log(playerPreviousLocation);
                //Debug.Log(this.gameObject.transform.position);
                _enemyStats.searchingTime = _enemyStats.searchingTimeStart;
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

        virtual protected void DefeatedManager()
        {
            // DefeatedManager will be differen't for the two enemies

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

        protected bool CanAttackPlayer()
        {
            if (_enemyAttackRadius.playerInRadius)
            {
                Debug.Log("InsideRadius");
                return true;
            }

            else
            {
                // Resets enemy attack
                _enemyStats.timeToAttack = _enemyStats.timeToAttackStart;
                Debug.Log("Outside radius");
                return false;
            }
        }

        // Made by Kyle
        //--------------------------------------------------------
        void FindPlayerRef()
        {
            if (playerObj != null) return;
            playerObj = GameObject.Find("Player");
        }

        void ResetRun()
        {
            this.gameObject.transform.position = NewGamePos;
        }

        void ReloadToSave()
        {
            this.gameObject.transform.position = SaveGamePos;
        }
        //--------------------------------------------------------
    }
}
