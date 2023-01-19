using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    //Code by Tobias
    public class SmallEnemyFish : Enemy
    {
        // Start is called before the first frame update
        void Start()
        {
            isAlive = true;
            enemyState = EnemyState.patrolling;
        }

        // Edmund's Audio... yikes - Edmund
        override protected void PlayAttackSound()
        {
            AM.PlaySoundSmallBite(source);
        }

        protected override void PlayAgroSound()
        {
            // TBA
        }
    }
}
