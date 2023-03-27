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
            repairTutorialPipe,
            enterMainHab,
            repairRedPipe,
            repairGreenPipe,
            activateReactor,
            goToElevator
        }

        public Objectives objective;

        [Header("Objectives")]
        [SerializeField] private bool[] isObjectiveComplete = new bool[System.Enum.GetNames(typeof(Objectives)).Length];
        [SerializeField] private string[] objectiveText = new string[System.Enum.GetNames(typeof(Objectives)).Length];
        public int numberOfObjectives;
        private string outgoingText;
        public float elapsedCountdownTime;
        [SerializeField] private float countdownTime;


        // Start is called before the first frame update
        void Start()
        {
            numberOfObjectives = isObjectiveComplete.Length;
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
            elapsedCountdownTime = countdownTime;
        }

        public string AssignObjectiveText()
        {
            outgoingText = "Objectives: ";

            for (int i = 0; i < isObjectiveComplete.Length; i++)
            {
                if (!isObjectiveComplete[i] && i == (int)Objectives.repairTutorialPipe)
                {
                    outgoingText += "\r\n" + objectiveText[i];
                }
                else if (!isObjectiveComplete[i] && i == (int)Objectives.enterMainHab && isObjectiveComplete[(int)Objectives.repairTutorialPipe])
                {
                    outgoingText += "\r\n" + objectiveText[i];
                }
                else if (!isObjectiveComplete[i] && i != (int)Objectives.goToElevator && isObjectiveComplete[(int)Objectives.repairTutorialPipe] && isObjectiveComplete[(int)Objectives.enterMainHab])
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

        private void FinalObjCountdown()
        {
            if (elapsedCountdownTime <= 0)
            {
                elapsedCountdownTime = 0;
                return;
            }

            elapsedCountdownTime -= Time.deltaTime;

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
