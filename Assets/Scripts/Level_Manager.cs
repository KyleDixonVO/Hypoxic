using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Manager : MonoBehaviour
{
    public static Level_Manager LM;
    public List<Scene> openScenes;
    
    private void Awake()
    {
        if (LM == null)
        {
            LM = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (LM != this && LM != null)
        {
            Destroy(this.gameObject);
        }
        openScenes = new List<Scene>();
    }

    private void Update()
    {
        MaintainSceneLedger();
    }

    private void MaintainSceneLedger()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (!openScenes.Contains(SceneManager.GetSceneAt(i)))
            {
                openScenes.Add(SceneManager.GetSceneAt(i));
            }
            else if (openScenes[i] != SceneManager.GetSceneAt(i))
            {
                openScenes.RemoveAt(i);
            }
            
        }
    }

    private bool IsSceneOpen(string sceneName)
    {
        for (int i =0; i < openScenes.Count; i++)
        {
            if (openScenes[i].name == sceneName) return true;
            Debug.Log("Scene is already open: " + sceneName);
        }

        return false;
    }

    public void LoadMainHab()
    {
        if (IsSceneOpen("Edmund-TransitionTest")) return;
        //Debug.Log("Loading Main Hab");
        SceneManager.LoadScene("Edmund-TransitionTest");
    }

    public void LoadOutside()
    {
        if (IsSceneOpen("Edmund-Test")) return;
        //Debug.Log("Loading Outside");
        SceneManager.LoadScene("Edmund-Test");
    }

    public void LoadMainMenu()
    {
        if (IsSceneOpen("MainMenu")) return;
        //Debug.Log("Loading Main Menu");
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGameplay()
    {
        
        if (IsSceneOpen("Gameplay")) return;
        //Debug.Log("Loading Gameplay");
        SceneManager.LoadScene("Gameplay");
        if (IsSceneOpen("Edmund-Test")) return;
        //Debug.Log("Loading Edmund-Test");
        LoadSceneAdditive("Edmund-Test");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadSceneAdditive(string sceneName)
    {
        if (IsSceneOpen(sceneName)) return;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void UnloadSceneAsync(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
