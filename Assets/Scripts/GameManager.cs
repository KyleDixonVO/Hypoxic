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
        if (gameManager == this) LoadMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        EvaluateState();
    }

    public void LoadMainMenu()
    {
        _gameState = gameStates.menu;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGameplay()
    {
        _gameState = gameStates.gameplay;
        SceneManager.LoadScene("Gameplay");
    }

    public void PauseGame()
    {
        _gameState = gameStates.paused;
    }

    public void LoadCutscene()
    {
        _gameState = gameStates.cutscene;
    }

    public void ExitGame()
    {
        Application.Quit();
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
