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

    [Header("Objectives")]
    [SerializeField] private bool objectiveOneComplete = false;
    [SerializeField] private bool objectiveTwoComplete = false;
    [SerializeField] private bool objectiveThreeComplete = true;
    [SerializeField] private bool finalObjectiveComplete = false;
    [SerializeField] private int numberOfObjectivesComplete = 0;
    [SerializeField] private int savedObjectivesComplete = 0;
    [SerializeField] private int objectivesToWin = 4;
    [SerializeField] private bool gameWon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckObjectiveCompletion();
    }

    public void ObjectiveOneComplete()
    {
        if (objectiveOneComplete) return;
        objectiveOneComplete = true;
        numberOfObjectivesComplete++;
    }

    public void ObjectiveTwoComplete()
    {
        if (objectiveTwoComplete) return;
        objectiveTwoComplete = true;
        numberOfObjectivesComplete++;
    }

    public void ObjectiveThreeComplete()
    {
        if (objectiveThreeComplete) return;
        objectiveThreeComplete = true;
        numberOfObjectivesComplete++;
    }

    public void FinalObjectiveComplete()
    {
        if (!objectiveOneComplete || !objectiveTwoComplete || !objectiveThreeComplete) return;
        if (finalObjectiveComplete) return;
        finalObjectiveComplete = true;
        numberOfObjectivesComplete++;
    }

    void CheckObjectiveCompletion()
    {
        if (numberOfObjectivesComplete >= objectivesToWin)
        {
            gameWon = true;
        }   
    }

    public bool GameWon()
    {
        return gameWon;
    }

    public void ResetRun()
    {
        gameWon = false;
        numberOfObjectivesComplete = 0;
        objectiveOneComplete = false;
        objectiveTwoComplete = false;
        objectiveThreeComplete = true;
        finalObjectiveComplete = false;
        savedObjectivesComplete = 0;
    }
}
