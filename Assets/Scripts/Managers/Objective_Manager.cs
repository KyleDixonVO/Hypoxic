using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
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
            anotherObjective,
            anotherObjectiveAgain,
            goToElevator
        }

        public Objectives objective;

        [Header("Objectives")]
        [SerializeField] private bool[] isObjectiveComplete = new bool[System.Enum.GetNames(typeof(Objectives)).Length];
        [SerializeField] private string[] objectiveText = new string[System.Enum.GetNames(typeof(Objectives)).Length];
        private string outgoingText;


        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
        }

        public void UpdateObjectiveCompletion(int objectiveNumber) // Objective_Manager.objectives.yourObjective -- (Objective_Manager.Objectives)yourNumber
        {
            if (objectiveNumber == (int)Objectives.goToElevator)
            {
                for (int i = 0; i < isObjectiveComplete.Length - 1; i++)
                {
                    if (!isObjectiveComplete[i]) return;
                }

                //if (!isObjectiveComplete[(int)Objectives.repairFirstPipe]
                //    || !isObjectiveComplete[(int)Objectives.repairSecondPipe]
                //    || !isObjectiveComplete[(int)Objectives.repairThirdPipe])
                //    return;
            }

            if (isObjectiveComplete[objectiveNumber]) return;
            isObjectiveComplete[objectiveNumber] = true;
            //Debug.Log(objectiveNumber + " " + isObjectiveComplete[objectiveNumber]);
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
                else if (i == (int)Objectives.goToElevator && CanCompleteFinalObjective())
                {
                    outgoingText += "\r\n" + objectiveText[i];
                }
            }
            return outgoingText;
        }

        private bool CanCompleteFinalObjective()
        {
            for(int i = 0; i < System.Enum.GetNames(typeof(Objectives)).Length; i++)
            {
                if (!isObjectiveComplete[i]) return false;
            }
            return true;
        }

        public void LoadObjectiveStates()
        {
            Data_Manager.dataManager.UpdateObjectiveManager();
        }
    }

}
