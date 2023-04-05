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
            gameState = _gameState;
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
                    PDA_LockMovement();
                    SwitchEndgame();
                    break;

                case gameStates.paused:
                    ReturnToGameplay();
                    break;

                case gameStates.cutscene:
                
                    break;

                case gameStates.menu:
                    Data_Manager.dataManager.LoadGlobalData();
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
            FirstPersonController_Sam.fpsSam.UnlockPlayerCamera();
            _gameState = gameStates.paused;
        }

        void PDA_LockMovement()
        {
            if (UI_Manager.ui_Manager.PDAOpen() && _gameState == gameStates.gameplay)
            {
                FirstPersonController_Sam.fpsSam.LockPlayerMovement();
                FirstPersonController_Sam.fpsSam.UnlockPlayerCamera();
            }
            else if (!UI_Manager.ui_Manager.PDAOpen() && _gameState == gameStates.gameplay && !PlayerStats.playerStats.IsDead())
            {
                FirstPersonController_Sam.fpsSam.UnlockPlayerMovement();
                FirstPersonController_Sam.fpsSam.LockPlayerCamera();
            }
        }

        public void GamestateCutscene()
        {
            _gameState = gameStates.cutscene;
            if (FirstPersonController_Sam.fpsSam != null)
            {
                FirstPersonController_Sam.fpsSam.LockPlayerMovement();
                FirstPersonController_Sam.fpsSam.LockPlayerCamera();
            }
        }

        public void GamestateGameplay()
        {
            _gameState = gameStates.gameplay;
            //Level_Manager.LM.LoadOutside();
            if (FirstPersonController_Sam.fpsSam != null)
            {
                FirstPersonController_Sam.fpsSam.UnlockPlayerMovement();
                FirstPersonController_Sam.fpsSam.LockPlayerCamera();
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
            Debug.Log("Returning to gameplay");
            UI_Manager.ui_Manager.SwitchGameplay();
            _gameState = gameStates.gameplay;
            FirstPersonController_Sam.fpsSam.UnlockPlayerMovement();
            FirstPersonController_Sam.fpsSam.LockPlayerCamera();
        }

        public void LoadLastSave()
        {
            //loads players last save
            Data_Manager.dataManager.LoadFromPlayerData();
            Objective_Manager.objective_Manager.LoadObjectiveStates();
            Data_Manager.dataManager.LoadFromEnemyData();
            Data_Manager.dataManager.LoadFromInteractableData();
            //Enemy_Manager.enemy_Manager.LoadEnemyStates();
            Enemy_Manager.enemy_Manager.SetShouldLoadTrue();
            Enemy_Manager.enemy_Manager.ResetToLastSave();
            Interactable_Manager.interactable_manager.ReloadToSavePositions();

            if (FirstPersonController_Sam.fpsSam == null) return;
            FirstPersonController_Sam.fpsSam.LoadCharacterState();
            PlayerStats.playerStats.LoadPlayerStats();
        }

        public void SaveGame()
        {
            //saves the game when at a checkpoint
            FirstPersonController_Sam.fpsSam.SaveCharacterState();
            Data_Manager.dataManager.PlayerAndObjectiveDataToDataManager();
            Enemy_Manager.enemy_Manager.SetSavePositions();
            Interactable_Manager.interactable_manager.SetSavePositions();
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
            InputManager.inputManager.ResetEscape();
            FirstPersonController_Sam.fpsSam.ResetRun();
            PlayerStats.playerStats.ResetRun();
            Object_Manager.object_Manager.ResetForNewRun();
            Interactable_Manager.interactable_manager.ResetForNewRun();
            Enemy_Manager.enemy_Manager.ResetForNewRun();
            PlayerInventory.playerInventory.ResetForNewRun();
            UI_Manager.ui_Manager.ResetForNewRun();

            if (FirstPersonController_Sam.fpsSam.GetComponentInChildren<MinimapPing>() == null) return;
            FirstPersonController_Sam.fpsSam.GetComponentInChildren<MinimapPing>().ResetForNextRun();
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
