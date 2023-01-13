using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager ui_Manager;

    [Header("Main Menu Assets")]
    [SerializeField] private TMP_Text textStartTitle;
    [SerializeField] private Button buttonStartGame;
    [SerializeField] private Button buttonNewGame;
    [SerializeField] private Button buttonOptions;
    [SerializeField] private Button buttonExit;

    [Header("Gameplay Assets")]
    [SerializeField] private Slider suitPower;
    [SerializeField] private Slider healthSlider;

    [Header("Pause Assets")]
    [SerializeField] private TMP_Text textPauseTitle;
    [SerializeField] private Button buttonResumeGame;
    [SerializeField] private Button buttonExitGame;

    [Header("Options Assets")]
    private ActiveUI canvasReturnFromOptions;
    [SerializeField] private TMP_Text textOptionsTitle;
    [SerializeField] private Button buttonOptionsBack;
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSFX;

    [Header("Credits Assets")]
    public TMP_Text textCreditsTitle;
    public TMP_Text textCreditsBody;

    [Header("New Game Assets")]
    [SerializeField] private TMP_Text textNewGame;
    [SerializeField] private Button buttonConfirmNG;
    [SerializeField] private Button buttonCancelNG;

    [Header("General Variables")]
    [SerializeField] private ActiveUI activeCanvas;
    [SerializeField] private Canvas[] canvasArray;

    public enum ActiveUI
    { 
        MainMenu,
        Gameplay,
        Pause,
        Options,
        Credits,
        NewGame
    }

    private void Awake()
    {
        if (ui_Manager == null)
        {
            ui_Manager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (ui_Manager != null && ui_Manager != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        activeCanvas = ActiveUI.MainMenu;
    }

    // Update is called once per frame
    void Update()
    {
        CheckActiveCanvas();
    }

    void CheckActiveCanvas()
    {
        //Debug.Log(activeCanvas);
        for (int i = 0; i < canvasArray.Length; i++)
        {
            if (i == (int)activeCanvas)
            {
                canvasArray[i].gameObject.SetActive(true);
                continue;
            }

            if (activeCanvas == ActiveUI.Pause && i == (int)ActiveUI.Gameplay) continue;
            canvasArray[i].gameObject.SetActive(false);
        }

        switch (activeCanvas)
        {
            case ActiveUI.MainMenu:
                
                break;

            case ActiveUI.Gameplay:
                if (FirstPersonController_Sam.fpsSam == null) return;
                healthSlider.maxValue = FirstPersonController_Sam.fpsSam.maxPlayerHealth;
                suitPower.maxValue = FirstPersonController_Sam.fpsSam.maxSuitPower;

                healthSlider.value = FirstPersonController_Sam.fpsSam.playerHealth;
                suitPower.value = FirstPersonController_Sam.fpsSam.suitPower;
                break;

            case ActiveUI.Pause:
                //if (!InputManager.inputManager.escapePressed)
                //{
                //    SwitchGameplay();
                //}
                break;

            case ActiveUI.Options:
                break;

            case ActiveUI.Credits:
                break;

            case ActiveUI.NewGame:
                break;
        }
    }

    public void SwitchGameplay()
    {
        activeCanvas = ActiveUI.Gameplay;
    }

    public void SwitchPause()
    {
        activeCanvas = ActiveUI.Pause;
    }

    public void SwitchOptions()
    {
        activeCanvas = ActiveUI.Options;
    }

    public void SwitchCredits()
    {
        activeCanvas = ActiveUI.Credits;
    }

    public void SwitchNewGame()
    {
        activeCanvas = ActiveUI.NewGame;
    }

    //do not make public
    public void SwitchMainMenu()
    {
        activeCanvas = ActiveUI.MainMenu;
    }
}
