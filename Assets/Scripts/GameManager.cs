using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Level_Manager.LM.LoadMainMenu();
        RandomSoundsManager.RSM.LoadVolumePrefs();
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
        Debug.Log("Gamestate: " + _gameState);
        switch (_gameState) 
        {
            case gameStates.gameplay:
                if (InputManager.inputManager.escapePressed)
                {
                    Debug.Log("Game Paused");
                    UI_Manager.ui_Manager.SwitchPause();
                    GamestatePause();
                    FirstPersonController_Sam.fpsSam.LockPlayerMovement();
                }
                break;

            case gameStates.paused:
                if (!InputManager.inputManager.escapePressed)
                {
                    ReturnToGameplay();
                }
                break;

            case gameStates.cutscene:
                
                break;

            case gameStates.menu:
                RandomSoundsManager.RSM.UpdateVolumePrefs();
                break;
        }

    }

    public void GamestatePause()
    {
        _gameState = gameStates.paused;
    }

    public void GamestateCutscene()
    {
        _gameState = gameStates.cutscene;
    }

    public void GamestateGameplay()
    {
        _gameState = gameStates.gameplay;
        Level_Manager.LM.LoadGameplay();
        if (FirstPersonController_Sam.fpsSam != null)
        {
            FirstPersonController_Sam.fpsSam.UnlockPlayerMovement();
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
        Debug.Log("Returning to gameplay");
        InputManager.inputManager.ResetEscape();
        UI_Manager.ui_Manager.SwitchGameplay();
        _gameState = gameStates.gameplay;
        FirstPersonController_Sam.fpsSam.UnlockPlayerMovement();
    }


}
