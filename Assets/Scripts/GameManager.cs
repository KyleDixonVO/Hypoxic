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
        }
        else if (gameManager != this && gameManager != null)
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        EvaluateState();
    }

    void LoadMainMenu()
    {
        _gameState = gameStates.menu;
        SceneManager.LoadScene("MainMenu");
    }

    void LoadGameplay()
    {
        _gameState = gameStates.gameplay;
        SceneManager.LoadScene("Gameplay");
    }

    void PauseGame()
    {
        _gameState = gameStates.paused;
    }

    void LoadCutscene()
    {
        _gameState = gameStates.cutscene;
    }

    void EvaluateState()
    {
        gameState = _gameState;
        switch (_gameState) 
        {
            case gameStates.gameplay:
                
                break;

            case gameStates.paused:

                break;

            case gameStates.cutscene:
                
                break;

            case gameStates.menu:
                
                break;


        }

    }


}
