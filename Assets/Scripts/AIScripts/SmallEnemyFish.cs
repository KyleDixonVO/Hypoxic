using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class SmallEnemyFish : Enemy
    {
        // Start is called before the first frame update
        protected void Start()
        {
            enemyState = EnemyState.chasing;
        }

    }
}
