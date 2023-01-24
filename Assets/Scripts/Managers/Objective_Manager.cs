using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective_Manager : MonoBehaviour
{
    public static Objective_Manager objective_Manager;

    private void Awake()
    {
        if (objective_Manager == null)
        {
            objective_Manager = this;
            DontDestroyOnLoad(this);
        }
        else if (objective_Manager != null && objective_Manager != this)
        {
            Destroy(this.gameObject);
        }
    }

    public enum Objectives 
    { 
        repairFirstPipe,
        repairSecondPipe,
        repairThirdPipe,
        goToElevator
    }

    public Objectives objective;

    [Header("Objectives")]
    [SerializeField] private bool[] isObjectiveComplete;
    [SerializeField] private string[] objectiveText;
    private string outgoingText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateObjectiveCompletion((int)Objectives.repairThirdPipe);
    }

    public void UpdateObjectiveCompletion(int objectiveNumber)
    {
        if (objectiveNumber == (int)Objectives.goToElevator)
        {
            if (!isObjectiveComplete[(int)Objectives.repairFirstPipe]
                || !isObjectiveComplete[(int)Objectives.repairSecondPipe]
                || !isObjectiveComplete[(int)Objectives.repairThirdPipe])
                return;
        }

        if (isObjectiveComplete[objectiveNumber]) return;
        isObjectiveComplete[objectiveNumber] = true;
        Debug.Log(objectiveNumber + " " + isObjectiveComplete[objectiveNumber]);
    }

    public bool IfWonGame()
    {
        for (int i = 0; i < isObjectiveComplete.Length; i++)
        {
            if (!isObjectiveComplete[i]) return false;
        }
        return true;
    }

    private void ResetObjectives()
    {
        for (int i = 0; i < isObjectiveComplete.Length; i++)
        {
            isObjectiveComplete[i] = false;
            Debug.Log(i + " " + isObjectiveComplete[i]);
        }
    }

    public bool GetObjectiveState(Objectives objective)
    {
        if (isObjectiveComplete[(int)objective]) return true;
        return false;
    }

    public void ResetRun()
    {
        ResetObjectives();
    }

    public string AssignObjectiveText()
    {
        outgoingText = "Objectives: ";

        for (int i = 0; i < isObjectiveComplete.Length; i++)
        {
            if (!isObjectiveComplete[i] && i != (int)Objectives.goToElevator)
            {
                outgoingText += "\r\n" + objectiveText[i];
            }
            else if (i == (int)Objectives.goToElevator)
            {
                if (!isObjectiveComplete[(int)Objectives.repairFirstPipe]
                || !isObjectiveComplete[(int)Objectives.repairSecondPipe]
                || !isObjectiveComplete[(int)Objectives.repairThirdPipe]) continue;

                outgoingText += "\r\n" + objectiveText[i];
            }
        }
        return outgoingText;
    }

    public void LoadObjectiveStates()
    {
        Data_Manager.dataManager.UpdateObjectiveManager();
    }


}
