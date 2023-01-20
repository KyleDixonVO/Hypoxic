using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairObject : MonoBehaviour
{
    [SerializeField] Vector3 repairDestination;
    [SerializeField] float repairTime = 5.0f;
    [SerializeField] float elapsedRepairTime;
    [SerializeField] float repairDistance = 2.0f;
    [SerializeField] bool repaired = false;
    [SerializeField] int objectiveNumber;

    public RepairTarget targetObject;

    // Start is called before the first frame update
    void Start()
    {
        repairDestination = targetObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckRepairStatus();
        Repair();
    }

    void Repair()
    {
        //Debug.Log(Vector3.Distance(this.gameObject.transform.position, repairDestination));
        if (WithinRepairRange() && InputManager.inputManager.rPressed && this.GetComponent<HeavyObject>().isHeld)
        {
            elapsedRepairTime += Time.deltaTime;
            Debug.Log(elapsedRepairTime / repairTime);
        }
        else
        {
            elapsedRepairTime = 0;
        }

        if (elapsedRepairTime >= repairTime)
        {
            repaired = true;
        }
    }

    void CheckRepairStatus() 
    { 
        if (repaired)
        {
            targetObject.RepairedObject();
            this.GetComponent<HeavyObject>().ForceDropObject();
            this.gameObject.SetActive(false);
            CompleteAssignedObjective();
        }
    }

    void CompleteAssignedObjective()
    {
        switch (objectiveNumber) 
        {
            case 1:
                Objective_Manager.objective_Manager.ObjectiveOneComplete();
                break;

            case 2:
                Objective_Manager.objective_Manager.ObjectiveTwoComplete();
                break;

            case 3:
                Objective_Manager.objective_Manager.ObjectiveThreeComplete();
                break;
        }

    }

    public bool WithinRepairRange()
    {
        if (Vector3.Distance(this.gameObject.transform.position, repairDestination) < repairDistance) return true;
        return false;
    }

}
