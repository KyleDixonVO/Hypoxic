using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace UnderwaterHorror
{
    [Serializable]
    //Code by Tobias & Kyle
    public class Enemy : MonoBehaviour
    {

        public bool isAlive;
        public enum EnemyState
        {
            patrolling,
            alerted,
            chasing,
            attacking,
            searching,
            defeated
        }

        public EnemyState enemyState;

        [Header("Scripts")]
        [SerializeField] public EnemyStats _enemyStats;

        [Header("GameObjects")]
        [SerializeField] protected List<GameObject> patrolPoints = new List<GameObject>();
        [SerializeField] public string patrolPointParentName;
        [SerializeField] private GameObject patrolPointParent;

        [Header("Place desired patrol point here")]
        [SerializeField] public int currentPatrolPoint;

        [Header("NavMesh")]
        [SerializeField] protected NavMeshAgent agent;

        [Header("FOVRaycast")]
        [SerializeField] protected LayerMask layerMasks;

        [Header("Saved Vectors")]
        [SerializeField] private Vector3 newGamePos;
        [SerializeField] public Vector3 saveGamePos;
        protected Vector3 playerPreviousLocation;

        // lil addon by edmund
        [Header("AudioSource")]
        [SerializeField] protected AudioSource source;

        public bool searching = false;
        private Vector3 lookTarget;

        private void Start()
        {
            isAlive = true;
            enemyState = EnemyState.patrolling;
        }

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
                FindPatrolPoints();
                // Makes it so the enemy can bite you immediatly on contact
                // But have to recharge after a single bite
                _enemyStats.timeToAttack -= Time.deltaTime;

                EnemyStateManager();
                Debug.Log(enemyState);
            }
        }

        void EnemyStateManager()
        {
            if (_enemyStats.health <= 0)
            {
                enemyState = EnemyState.defeated;
            }

            switch (enemyState)
            {
                //-----------------------------------------  
                case EnemyState.patrolling:
                    PatrollingManager();
                    if (HasLineOfSight() && InFOVCone() && WithinRange(_enemyStats.GetDetectionDistance(), agent.transform.position, FirstPersonController_Sam.fpsSam.transform.position))
                    {
                        Debug.LogWarning("Switching to chasing state from patrolling");
                        //Replace PlayAggroSound with the generic PlaySound from Audiomanager
                        //PlayAgroSound();
                        enemyState = EnemyState.chasing;
                    }
                    else if (HasLineOfSight() && WithinRange(_enemyStats.GetDetectionDistance() / 2, agent.transform.position, FirstPersonController_Sam.fpsSam.transform.position))
                    {
                        Debug.LogWarning("Switching to alert state from patrolling");
                        enemyState = EnemyState.alerted;
                    }
                    break;
                //-----------------------------------------  
                case EnemyState.alerted:
                    // Looks Direction it was alerted to after a delay
                    AlertedManager();
                    if (HasLineOfSight() && InFOVCone() && WithinRange(_enemyStats.GetDetectionDistance(), agent.transform.position, FirstPersonController_Sam.fpsSam.transform.position))
                    {
                        Debug.LogWarning("Switching to chasing state from alerted");
                        agent.isStopped = false;
                        enemyState = EnemyState.chasing;
                    }
                    else if (agent.isStopped && _enemyStats.elapsedAlertTime >= _enemyStats.alertCheckingTime)
                    {
                        Debug.LogWarning("Switching to patrolling state from alerted");
                        agent.isStopped = false;
                        enemyState = EnemyState.patrolling;
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
                    else if (WithinRange(_enemyStats.attackStateRadius, agent.transform.position, FirstPersonController_Sam.fpsSam.transform.position))
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
                    if (!WithinRange(_enemyStats.attackStateRadius, agent.transform.position, FirstPersonController_Sam.fpsSam.transform.position))
                    {
                        Debug.LogWarning("Switching to chasing state from attacking");
                        enemyState = EnemyState.chasing;
                    }
                    break;
                //-----------------------------------------          
                case EnemyState.searching:
                    SearchingManager();
                    if (HasLineOfSight() && InFOVCone())
                    {
                        Debug.LogWarning("Switching to chasing state from searching");
                        searching = false;
                        enemyState = EnemyState.chasing;
                    }
                    else if (_enemyStats.searchingTime <= 0)
                    {
                        Debug.LogWarning("Switching to patrolling state from searching");
                        searching = false;
                        enemyState = EnemyState.patrolling;
                    }
                    break;
                //-----------------------------------------  
                case EnemyState.defeated:
                    // Starts dying animation
                    DefeatedManager();
                    break;
                    //-----------------------------------------  
            }
        }

        virtual protected void PatrollingManager()
        {
            agent.speed = _enemyStats.patrolSpeed;
            // Follow patrol points in random order
            if (patrolPointParent == null) return;
            if (agent.destination != patrolPoints[currentPatrolPoint].transform.position)
            {
                agent.SetDestination(patrolPoints[currentPatrolPoint].transform.position);
            }

            Vector3 targetPos = new Vector3(patrolPoints[currentPatrolPoint].transform.position.x, 0, patrolPoints[currentPatrolPoint].transform.position.z);
            Vector3 agentPos = new Vector3(agent.transform.position.x, 0, agent.transform.position.z);
            //--------------------------------------------------------------

            if (!WithinRange(_enemyStats.patrolRadius, targetPos, agentPos))
            {
                _enemyStats.patrolRandomWaitTimer = UnityEngine.Random.Range(0, _enemyStats.patrolRandomWaitTimerWeight);
                return;
            }

            if (_enemyStats.patrolRandomWaitTimer <= 0)
            {
                int randomPoint = UnityEngine.Random.Range(0, patrolPoints.Count);
                if (currentPatrolPoint != randomPoint) currentPatrolPoint = randomPoint;
                else if (currentPatrolPoint + 1 < patrolPoints.Count) currentPatrolPoint++;
                else if (currentPatrolPoint - 1 >= 0) currentPatrolPoint--;

            }
            else _enemyStats.patrolRandomWaitTimer -= Time.deltaTime;
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
                PlayerStats.playerStats.playerHit = true;
                _enemyStats.timeToAttack = _enemyStats.timeToAttackStart;
            }
        }

        void AlertedManager() 
        {
            if (agent.isStopped == false)
            {
                agent.isStopped = true;
                lookTarget = FirstPersonController_Sam.fpsSam.transform.position - agent.transform.position;
                lookTarget.Normalize();
                _enemyStats.elapsedAlertTime = 0;
            }

            this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, Quaternion.LookRotation(lookTarget), agent.angularSpeed * Time.deltaTime);
            if (Vector3.Angle(transform.forward, lookTarget) > 5) return;
            if (_enemyStats.elapsedAlertTime < _enemyStats.alertCheckingTime) 
            {
                _enemyStats.elapsedAlertTime += Time.deltaTime;
                return;
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

            if (WithinRange(1, previousPos, agentPos) && !searching)
            {
                _enemyStats.searchingTime = _enemyStats.searchingTimeStart;
                searching = true;
            }
            else if (searching)
            {
                // Searching Movement...
                Debug.Log(_enemyStats.searchingTime);
                _enemyStats.searchingTime -= Time.deltaTime;
            }
        }

        virtual protected void DefeatedManager()
        {
            // DefeatedManager will be different for the two enemies
            // This is the defeated manager for the small fish, smallFish class has been deleted

            // Death Time
            agent.isStopped = true;
            _enemyStats.dyingTime -= Time.deltaTime;
            isAlive = false;

            if (_enemyStats.dyingTime <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }

        protected bool HasLineOfSight()
        {
            Vector3 raycastDir = PlayerStats.playerStats.transform.position;
            if (!Physics.Linecast(this.gameObject.transform.position, raycastDir, layerMasks))
            {
                Debug.Log("Has Line Of Sight ");
                return true;
            }
            else
            {
                Debug.Log("No Visual ");
                return false;
            }
        }

        protected bool InFOVCone()
        {
            
            Debug.DrawRay(agent.transform.position, agent.transform.forward * _enemyStats.visionRayLength);
            Vector3 angleVector = FirstPersonController_Sam.fpsSam.transform.position - transform.position;
            if (Vector3.Angle(angleVector, agent.transform.forward) < _enemyStats.visionConeHalved) 
            {
                //Debug.Log("In FOV Cone " + Vector3.Angle(angleVector, agent.transform.forward));
                Debug.DrawLine(FirstPersonController_Sam.fpsSam.transform.position, agent.transform.position, Color.green);
                return true; 
            }
            //Debug.Log("Outside FOV Cone " + Vector3.Angle(angleVector, agent.transform.forward));
            Debug.DrawLine(FirstPersonController_Sam.fpsSam.transform.position, agent.transform.position, Color.red);
            return false;
        }

        protected bool WithinRange(float rangeToCheck, Vector3 firstPos, Vector3 secondPos)
        {
            if (Vector3.Distance(firstPos, secondPos) < rangeToCheck)
            {
                //Debug.Log("Within Range " + Vector3.Distance(firstPos, secondPos) + " " + rangeToCheck + " " + _enemyStats.GetDetectionDistance());
                return true;
            }
            //Debug.Log("Out Of Range" + Vector3.Distance(firstPos, secondPos) + " " + rangeToCheck + " " + _enemyStats.GetDetectionDistance());
            return false;
        }

        private void FindPatrolPoints()
        {
            if (GameObject.Find(patrolPointParentName) == null)
            {
                agent.destination = this.transform.position;
                patrolPoints.Clear();
                return;
            }
            patrolPointParent = GameObject.Find(patrolPointParentName);
            for (int i = 0; i < patrolPointParent.transform.childCount; i++)
            {
                if (patrolPoints.Contains(patrolPointParent.transform.GetChild(i).gameObject)) continue;
                patrolPoints.Add(patrolPointParent.transform.GetChild(i).gameObject);
            }
        }

        public virtual void ResetRun()
        {
            this.gameObject.transform.position = newGamePos;
            this._enemyStats.health = this._enemyStats.maxHealth;
            this.isAlive = true;
            this.enemyState = EnemyState.patrolling;
            this.searching = false;
        }

        public void SetSaveGamePos()
        {
            saveGamePos = this.gameObject.transform.position;
        }

        public void ReloadToSave()
        {
            this.gameObject.transform.position = saveGamePos;
        }
        //--------------------------------------------------------
    }
}
