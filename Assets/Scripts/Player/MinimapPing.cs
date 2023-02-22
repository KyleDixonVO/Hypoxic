using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror 
{
    public class MinimapPing : MonoBehaviour
    {
        [SerializeField] private Transform pingParent;
        [SerializeField] private Transform minimapRing;
        [SerializeField] private float range;
        [SerializeField] private float maxRange;
        [SerializeField] private float pingSpeed;
        [SerializeField] private List<Collider> pingHitList;
        [SerializeField] private List<GameObject> blipList;
        [SerializeField] private LayerMask pingMask;
        [SerializeField] private GameObject blipPrefab;

        // Start is called before the first frame update
        void Start()
        {
            pingHitList = new List<Collider>();
        }

        // Update is called once per frame
        void Update()
        {
            PingMap();
            FadeBlips();
        }

        public void PingMap()
        {
            if (!Level_Manager.LM.IsSceneOpen("Outside"))
            {
                range = 0;
                return;
            }
            else if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay || !InputManager.inputManager.tabPressed) return;

            range += pingSpeed * Time.deltaTime;
            if (range >= maxRange) 
            {
                range = 0f;
                pingHitList.Clear();
            } 
            pingParent.localScale = new Vector3(range, range);
            minimapRing.localScale = new Vector3(range * 2, range * 2);
            Collider[] pingArray = Physics.OverlapSphere(pingParent.position, range, pingMask);
            foreach(Collider raycastHit in pingArray)
            {
                if (!pingHitList.Contains(raycastHit))
                {
                    pingHitList.Add(raycastHit);
                    blipList.Add(GameObject.Instantiate(blipPrefab, new Vector3(raycastHit.transform.position.x, raycastHit.transform.position.y, raycastHit.transform.position.z), Quaternion.Euler(new Vector3(90, 0, 0))));
                }
            }
        }

        public void FadeBlips()
        {
            foreach(GameObject gameObject in blipList)
            {
                gameObject.GetComponent<BlipController>().FadeOut();
            }

            IList<GameObject> readOnlyBlipList = blipList.AsReadOnly();
            for(int i = 0; i < readOnlyBlipList.Count; i++)
            {
                if (!readOnlyBlipList[i].GetComponent<BlipController>().visible)
                {
                    Destroy(blipList[i]);
                    blipList.Remove(blipList[i]);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(pingParent.transform.position, range);
        }
    }
}

