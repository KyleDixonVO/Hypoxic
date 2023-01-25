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
        [SerializeField] protected EnemyStats _enemyStats;

        [Header("GameObjects")]
        [SerializeField] protected List<GameObject> patrolPoints = new List<GameObject>();
        [SerializeField] protected GameObject attackAnimation;

        [Header("Place desired patrol point here")]
        [SerializeField] protected GameObject currentPatrolPoint;
        
        [Header("NavMesh")]
        [SerializeField] protected NavMeshAgent agent;

        [Header("FOVRaycast")]
        [SerializeField] protected LayerMask layerMasks;

        [SerializeField] private Vector3 NewGamePos;
        [SerializeField] private Vector3 SaveGamePos;
        protected Vector3 playerPreviousLocation;

        // lil addon by edmund
        [Header("Refrences")]
        [SerializeField] protected AudioSource source;

        // Update is called once per frame
        protected void Update()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay)
            {
                agent.isStopped = true;
                return;
            }
            else
            {
                agent.isStopped = false;
            }

            // Makes it so the enemy can bite you immediatly on contact
            // But have to recharge after a single bite
            _enemyStats.timeToAttack -= Time.deltaTime;

            switch (enemyState)
            {
                //-----------------------------------------  
                case EnemyState.patrolling:
                    PatrollingManager();
                    if (HasLineOfSight() && WithinRange(_enemyStats.GetDetectionDistance(), transform.position, FirstPersonController_Sam.fpsSam.transform.position))
                    {
                        Debug.LogWarning("Switching to chasing state from patrolling");
                        PlayAgroSound();
                        enemyState = EnemyState.chasing;
                    }
                    break;
                //-----------------------------------------  
                case EnemyState.alerted:
                    // Looks Direction it was alerted to after a delay
                    if (HasLineOfSight() && WithinRange(_enemyStats.GetDetectionDistance(), transform.position, FirstPersonController_Sam.fpsSam.transform.position))
                    {
                        Debug.LogWarning("Switching to chasing state from alerted");
                        enemyState = EnemyState.chasing;
                    }
                    break;
                //-----------------------------------------  
                case EnemyState.chasing:
                    ChasingManager();
                    if (!HasLineOfSight())
                    {
                        Debug.LogWarning("Switching to searching state from chasing");
                        enemyState = EnemyState.searching;
                    }                       
                    else if (WithinRange(_enemyStats.attackStateRadius, transform.position, FirstPersonController_Sam.fpsSam.transform.position))
                    {
                        Debug.LogWarning("Switching to attaking state from chasing");
                        enemyState = EnemyState.attacking;
                    }
                    break;
                //-----------------------------------------  
                case EnemyState.attacking:
                    // Will still chase player
                    ChasingManager();
                    // attacks player when in range
                    AttackingManager();
                    if (!WithinRange(_enemyStats.attackStateRadius, transform.position, FirstPersonController_Sam.fpsSam.transform.position))
                    {
                        Debug.LogWarning("Switching to chasing state from attacking");
                        enemyState = EnemyState.chasing;
                    }
                    break;
                //-----------------------------------------          
                case EnemyState.searching:
                    SearchingManager();
                    if (HasLineOfSight())
                    {
                        Debug.LogWarning("Switching to chasing state from searching");
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

            Debug.Log(enemyState);
        }

        void PatrollingManager()
        {
            agent.speed = _enemyStats.patrolSpeed;
            // Follow patrol points in random order
            if (agent.destination != currentPatrolPoint.transform.position)
            {
                agent.SetDestination(currentPatrolPoint.transform.position);
            }

            // Fixes find Dest bug 
            Vector3 pointPos = currentPatrolPoint.transform.position;
            Vector3 agentPos = this.agent.transform.position;

            Vector3 pointPosDest = new Vector3(pointPos.x, 0, pointPos.z);
            Vector3 agentPosDest = new Vector3(agentPos.x, 0, agentPos.z);
            //--------------------------------------------------------------

            if (WithinRange(6, pointPosDest, agentPosDest))
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
        }

        void ChasingManager()
        {
            agent.speed = _enemyStats.chaseSpeed;
            // Chases after the players position
            agent.SetDestination(PlayerStats.playerStats.gameObject.transform.position);

            // Tracks player's previous location
            playerPreviousLocation = PlayerStats.playerStats.transform.position;           
            //---------------------------------------------------------
        }

        void AttackingManager()
        {
            if (_enemyStats.timeToAttack <= 0)
            {                               
                PlayerStats.playerStats.TakeDamage(_enemyStats.attackPower);
                _enemyStats.timeToAttack = _enemyStats.timeToAttackStart;               
            }
            else
            {

            }
        }

        void SearchingManager()
        {
            agent.speed = _enemyStats.searchingMovementSpeed;

            Vector3 previousPos = new Vector3(playerPreviousLocation.x, 0, playerPreviousLocation.z);
            Vector3 agentPos = new Vector3(agent.transform.position.x, 0, agent.transform.position.z);
            // ------------------------------------------------------------------------

            // Goes to last spot the player was last scene
            agent.SetDestination(playerPreviousLocation); //<--- playerPreviousLocation gets set in ChasingManager 

            if (!WithinRange(1, previousPos, agentPos))
            {                
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

        // Made by Kyle
        //--------------------------------------------------------

        protected bool HasLineOfSight()
        {
            Vector3 raycastDir = PlayerStats.playerStats.transform.position;
            Vector3 angleVector = FirstPersonController_Sam.fpsSam.transform.position - transform.position;
            Debug.DrawLine(FirstPersonController_Sam.fpsSam.transform.position, this.transform.position);
            Debug.DrawRay(this.transform.position, this.transform.forward * 100);
            if (!Physics.Linecast(this.gameObject.transform.position, raycastDir, layerMasks) && Vector3.Angle(angleVector, this.transform.forward) < 30)
            {
                Debug.Log("Has Line Of Sight " + Vector3.Angle(angleVector, this.transform.forward));
                return true;
            }

            else
            {
                Debug.Log("No Visual " + Vector3.Angle(angleVector, this.transform.forward));
                return false;
            }
        }

        protected bool WithinRange(float rangeToCheck, Vector3 firstPos, Vector3 secondPos)
        {
            if (Vector3.Distance(firstPos, secondPos) < rangeToCheck)
            {
                Debug.Log("Within Range " + Vector3.Distance(firstPos, secondPos) + " " + rangeToCheck + " " + _enemyStats.GetDetectionDistance());
                return true;
            }
            Debug.Log("Out Of Range" + Vector3.Distance(firstPos, secondPos) + " " + rangeToCheck + " " + _enemyStats.GetDetectionDistance());
            return false;
        }

        //Don't call these yet, they aren't implemented properly
        void ResetRun()
        {
            this.gameObject.transform.position = NewGamePos;
        }

        void ReloadToSave()
        {
            this.gameObject.transform.position = SaveGamePos;
        }
        //--------------------------------------------------------

        // Edmund Time
        // -------------------------------------------------------
        virtual protected void PlayAttackSound()
        {

        }

        virtual protected void PlayAgroSound()
        {

        }
        // -------------------------------------------------------
    }
}
