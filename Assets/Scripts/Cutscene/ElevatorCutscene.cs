using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class ElevatorCutscene : MonoBehaviour
    {
        Animator animator;

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
            GameManager.gameManager.gameState = GameManager.gameStates.cutscene;
        }
        public void GoToGameplay()
        {
            GameManager.gameManager.gameState = GameManager.gameStates.gameplay;
        }
    }
}

