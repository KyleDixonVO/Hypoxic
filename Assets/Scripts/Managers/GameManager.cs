using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnderwaterHorror
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager gameManager;
        public enum gameStates
        {
            gameplay,
            paused,
            cutscene,
            menu
        }

        [SerializeField] private gameStates _gameState;
        public gameStates gameState;

        private void Awake()
        {
            if (gameManager == null)
            {
                gameManager = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (gameManager != this && gameManager != null)
            {
                Destroy(this.gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            _gameState = gameStates.menu;
            Data_Manager.dataManager.LoadGlobalData();
            Level_Manager.LM.LoadMainMenu();
            AudioManager.audioManager.LoadVolumePrefs();
            UI_Manager.ui_Manager.SetOptionsSliderValues();
        }

        // Update is called once per frame
        void Update()
        {
            EvaluateState();
        }


        //Make a scene manager for these methods


        void EvaluateState()
        {
            gameState = _gameState;
            //Debug.Log("Gamestate: " + _gameState);
            switch (_gameState) 
            {
                case gameStates.gameplay:
                    GamestatePause();
                    SwitchEndgame();
                    break;

                case gameStates.paused:
                    ReturnToGameplay();
                    break;

                case gameStates.cutscene:
                
                    break;

                case gameStates.menu:
                    UpdateVolumePrefs();
                    DisplayEndgameText();
                    break;
            }

        }

        public void GamestatePause()
        {
            if (!InputManager.inputManager.escapePressed) return;
            
            Debug.Log("Game Paused");
            UI_Manager.ui_Manager.SwitchPause();
            FirstPersonController_Sam.fpsSam.LockPlayerMovement();
            _gameState = gameStates.paused;
            
        }

        public void GamestateCutscene()
        {
            _gameState = gameStates.cutscene;
        }

        public void GamestateGameplay()
        {
            _gameState = gameStates.gameplay;
            Level_Manager.LM.LoadOutside();
            if (FirstPersonController_Sam.fpsSam != null)
            {
                FirstPersonController_Sam.fpsSam.UnlockPlayerMovement();
                FirstPersonController_Sam.fpsSam.EnableCharacterController();
            }
        }

        public void GamestateMainMenu()
        {
            _gameState = gameStates.menu;
            Level_Manager.LM.LoadMainMenu();
            UI_Manager.ui_Manager.SwitchMainMenu();
            InputManager.inputManager.ResetEscape();
        }

        public void ReturnToGameplay()
        {
            if (InputManager.inputManager.escapePressed) return;
            Debug.Log("Returning to gameplay");;
            UI_Manager.ui_Manager.SwitchGameplay();
            _gameState = gameStates.gameplay;
            FirstPersonController_Sam.fpsSam.UnlockPlayerMovement();
        }

        public void LoadLastSave()
        {
            //loads players last save
            Data_Manager.dataManager.LoadFromPlayerData();
            Objective_Manager.objective_Manager.LoadObjectiveStates();

            if (FirstPersonController_Sam.fpsSam == null) return;
            FirstPersonController_Sam.fpsSam.LoadCharacterState();
            PlayerStats.playerStats.LoadPlayerStats();
        
        }

        public void SaveGame()
        {
            //saves the game when at a checkpoint
            FirstPersonController_Sam.fpsSam.SaveCharacterState();
            Data_Manager.dataManager.SaveToDataManager();
        }
        
        void DisplayEndgameText()
        {
            if (PlayerStats.playerStats == null) return;
            if (PlayerStats.playerStats.IsDead())
            {
                UI_Manager.ui_Manager.SwitchGameOverWin();
                UI_Manager.ui_Manager.SwitchDeathText();
            }
            else if (Objective_Manager.objective_Manager.IfWonGame())
            {
                UI_Manager.ui_Manager.SwitchGameOverWin();
                UI_Manager.ui_Manager.SwitchWinText();
            }
        }

        void SwitchEndgame()
        {
            if (PlayerStats.playerStats == null || Objective_Manager.objective_Manager == null) return;
            if (PlayerStats.playerStats.IsDead() || Objective_Manager.objective_Manager.IfWonGame())
            {
                _gameState = gameStates.menu;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        void UpdateVolumePrefs()
        {
            if (!UI_Manager.ui_Manager.OptionsOpen()) return;
            AudioManager.audioManager.UpdateVolumePrefs();
        }

        public void ResetForNewRun()
        {
            Objective_Manager.objective_Manager.ResetRun();
            InputManager.inputManager.ResetTab();
            if (FirstPersonController_Sam.fpsSam == null) return;
            FirstPersonController_Sam.fpsSam.ResetRun();
            PlayerStats.playerStats.ResetRun();
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
