using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class FinalObjectiveTrigger : MonoBehaviour
    {
        [SerializeField] GameObject lift;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("trigger in");
            Debug.Log("name: " + other.name);
            if (other.tag != "lift") return;
            Objective_Manager.objective_Manager.UpdateObjectiveCompletion((int)Objective_Manager.Objectives.goToElevator);        
        }

        private void OnTriggerStay(Collider other)
        {
            Debug.Log("trigger in");
            Debug.Log("name: " + other.name);
            if (other.tag != "Player" || other.transform.parent == lift.transform) return;
            other.transform.SetParent(lift.transform);
            FirstPersonController_Sam.fpsSam.DisableCharacterMovement();
        }
    }

}
