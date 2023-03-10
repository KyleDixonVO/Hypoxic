using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnderwaterHorror
{
    //Code by Tobias and Kyle
    public class BigEnemyFish : Enemy
    {
        [Header("Unique to big fish")]
        //used when small fish call or when player triggers objectives
        [SerializeField] private bool isAware = false;

        [SerializeField] private bool pointsListPopulated = false;
        [SerializeField] private int numberOfPointsNearPlayer = 3;
        [SerializeField] private List<DistSort> pointsOrderedByDist;
        [SerializeField] private List<DistSort> distSorts;

        private void Start()
        {
            pointsOrderedByDist = new List<DistSort>();
            distSorts = new List<DistSort>();
        }

        override protected void DefeatedManager()
        {
            // Fleeing Time
            _enemyStats.fleeingTime -= Time.deltaTime;

            agent.speed = _enemyStats.fleeingSpeed;
            agent.SetDestination(FirstPersonController_Sam.fpsSam.transform.position - transform.position);

            if (_enemyStats.fleeingTime <= 0)
            {
                enemyState = EnemyState.patrolling;
                _enemyStats.health = _enemyStats.maxHealth;
                _enemyStats.elapsedFleeingTime = _enemyStats.fleeingTime;
            }
        }

        override protected void PatrollingManager()
        {
            agent.speed = _enemyStats.patrolSpeed;
            if (isAware)
            {
                _enemyStats.elapsedAwareTime += Time.deltaTime;
                if (_enemyStats.elapsedAwareTime >= _enemyStats.awareTime)
                {
                    ClearPointsByDist();
                    isAware = false;
                    _enemyStats.elapsedAwareTime = 0;
                }
            }

            if (patrolPoints.Count <= 0) return;
            if (agent.destination != patrolPoints[currentPatrolPoint].transform.position)
            {
                agent.SetDestination(patrolPoints[currentPatrolPoint].transform.position);
            }

            Vector3 targetPos = new Vector3(patrolPoints[currentPatrolPoint].transform.position.x, 0, patrolPoints[currentPatrolPoint].transform.position.z);
            Vector3 agentPos = new Vector3(agent.transform.position.x, 0, agent.transform.position.z);
            //--------------------------------------------------------------

            if (!WithinRange(_enemyStats.patrolRadius, targetPos, agentPos))
            {
                _enemyStats.patrolRandomWaitTimer = Random.Range(0, _enemyStats.patrolRandomWaitTimerWeight);
                return;
            }

            if (_enemyStats.patrolRandomWaitTimer > 0)
            {
                _enemyStats.patrolRandomWaitTimer -= Time.deltaTime;
                return;
            }

            int randomPoint;
            if (isAware)
            {
                PopulatePointsNearPlayer();
                randomPoint = pointsOrderedByDist[Random.Range(0, numberOfPointsNearPlayer - 1)].initialListPlacement;
            }
            else
            {
                randomPoint = Random.Range(0, patrolPoints.Count);
            }

            if (currentPatrolPoint != randomPoint) currentPatrolPoint = randomPoint;
            else if (currentPatrolPoint + 1 < patrolPoints.Count) currentPatrolPoint++;
            else if (currentPatrolPoint - 1 >= 0) currentPatrolPoint--;
        }

        private void PopulatePointsNearPlayer()
        {
            if (pointsListPopulated) return;

            for(int i = 0; i < patrolPoints.Count; i++)
            {
                DistSort dist = new DistSort();
                dist.initialListPlacement = i;
                dist.distance = Vector3.Distance(FirstPersonController_Sam.fpsSam.transform.position, patrolPoints[i].transform.position);
                distSorts.Add(dist);
            }

            pointsOrderedByDist = distSorts.OrderBy(distSorts => distSorts.distance).ToList();

            pointsListPopulated = true;
        }

        private void ClearPointsByDist()
        {
            distSorts.Clear();
            pointsOrderedByDist.Clear();
            pointsListPopulated = false;
        }

        public void AlertBigFish()
        {
            if (isAware) return;
            isAware = true;
        }

        public override void ResetRun()
        {
            base.ResetRun();
            isAware = false;
        }
    }

    public struct DistSort
    {
        public DistSort(int placement, float dist)
        {
            initialListPlacement = placement;
            distance = dist;
        }
        public int initialListPlacement { get; set; }

        public float distance;



    }

}
