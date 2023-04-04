using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class ElevatorCutscene : MonoBehaviour
    {
        Animator animator;
        [SerializeField] GameObject lift;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            PauseCheck();
        }

        void PauseCheck()
        {
            if (GameManager.gameManager.gameState == GameManager.gameStates.paused || UI_Manager.ui_Manager.PDAOpen())
            {
                animator.speed = 0;
            }
            else if (GameManager.gameManager.gameState != GameManager.gameStates.paused || UI_Manager.ui_Manager.PDAOpen() == false && animator.speed != 1) animator.speed = 1;
        }

        public void GoToCutScene()
        {
            FirstPersonController_Sam.fpsSam.DisableCharacterMovement();
            FirstPersonController_Sam.fpsSam.gameObject.transform.SetParent(lift.transform);
        }
        public void GoToGameplay()
        {
            FirstPersonController_Sam.fpsSam.EnableCharacterMovement();
            FirstPersonController_Sam.fpsSam.gameObject.transform.SetParent(null);
            DontDestroyOnLoad(FirstPersonController_Sam.fpsSam);
        }

        public void EndGameObjective()
        {
            Objective_Manager.objective_Manager.UpdateObjectiveCompletion((int)Objective_Manager.Objectives.goToElevator);
        }
    }
}

