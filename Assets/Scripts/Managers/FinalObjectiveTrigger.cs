using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalObjectiveTrigger : MonoBehaviour
{
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
        if (other.tag != "Player") return;
        Objective_Manager.objective_Manager.UpdateObjectiveCompletion((int)Objective_Manager.Objectives.repairFirstPipe);
    }
}
