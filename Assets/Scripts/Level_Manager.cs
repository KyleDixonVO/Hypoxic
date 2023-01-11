using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Manager : MonoBehaviour
{
    public void LoadMainHab()
    {
        SceneManager.LoadScene("Edmund-TransitionTest");
    }

    public void LoadOutside()
    {
        SceneManager.LoadScene("Edmund-Test");
    }
}
