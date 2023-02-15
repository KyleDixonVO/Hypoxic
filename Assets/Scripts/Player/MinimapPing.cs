using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror 
{
    public class MinimapPing : MonoBehaviour
    {
        private Transform pingParent;
        [SerializeField] private float range;
        [SerializeField] private float maxRange;
        [SerializeField] private float pingSpeed;
        [SerializeField] private List<Collider> pingHitList;
        [SerializeField] private LayerMask pingMask;

        // Start is called before the first frame update
        void Start()
        {
            pingHitList = new List<Collider>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PingMap()
        {
            range += pingSpeed * Time.deltaTime;
            if (range >= maxRange) 
            {
                range = 0f;
                pingHitList.Clear();
            } 
            pingParent.localScale = new Vector3(range, range);
            RaycastHit[] pingArray = Physics.SphereCastAll(this.gameObject.transform.position, range, Vector3.zero, maxRange, pingMask);
            foreach(RaycastHit raycastHit in pingArray)
            {
                if (!pingHitList.Contains(raycastHit.collider))
                {
                    pingHitList.Add(raycastHit.collider);
                }
            }

        }
    }
}

