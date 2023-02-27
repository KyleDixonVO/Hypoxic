using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnderwaterHorror
{
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

        public bool IsSceneOpen(string sceneName)
        {
            for (int i =0; i < openScenes.Count; i++)
            {
                if (openScenes[i].name == sceneName) return true;
                //Debug.Log("Scene is already open: " + sceneName);
            }

            return false;
        }

        public void LoadMainHab()
        {
            if (IsSceneOpen("DemoBuildingInside")) return;
            //Debug.Log("Loading Main Hab");
            SceneManager.LoadScene("DemoBuildingInside");
            FirstPersonController_Sam.fpsSam.IndoorTransition();
        }

        public void LoadOutside()
        {
            if (IsSceneOpen("Outside")) return;
            Debug.Log("Loading Outside");
            SceneManager.LoadScene("Outside");
        }

        public void LoadMainMenu()
        {
            if (IsSceneOpen("MainMenu")) return;
            //Debug.Log("Loading Main Menu");
            SceneManager.LoadScene("MainMenu");
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

}
