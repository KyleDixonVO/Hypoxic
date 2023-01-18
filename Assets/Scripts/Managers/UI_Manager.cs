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
    [SerializeField] private Image playerHitEffect;

    [Header("Pause Assets")]
    [SerializeField] private TMP_Text textPauseTitle;
    [SerializeField] private Button buttonResumeGame;
    [SerializeField] private Button buttonExitGame;

    [Header("Options Assets")]
    private ActiveUI canvasReturnFromOptions;
    [SerializeField] private TMP_Text textOptionsTitle;
    [SerializeField] private TMP_Text textMasterSlider;
    [SerializeField] private TMP_Text textMusicSlider;
    [SerializeField] private TMP_Text textSFXSlider;
    [SerializeField] private Button buttonOptionsBack;
    [SerializeField] public Slider sliderMaster;
    [SerializeField] public Slider sliderMusic;
    [SerializeField] public Slider sliderSFX;

    [Header("Credits Assets")]
    [SerializeField] private TMP_Text textCreditsTitle;
    [SerializeField] private TMP_Text textCreditsBody;

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
        FindReferences();
    }

    // Update is called once per frame
    void Update()
    {
        CheckActiveCanvas();
    }

    void FindReferences()
    {
        textStartTitle = GameObject.Find("TextStartTitle").GetComponent<TMP_Text>();
        buttonStartGame = GameObject.Find("ButtonStartGame").GetComponent<Button>();
        buttonNewGame = GameObject.Find("ButtonNewGame").GetComponent<Button>();
        buttonOptions = GameObject.Find("ButtonOptions").GetComponent<Button>();
        buttonExit = GameObject.Find("ButtonExit").GetComponent<Button>();

        textPauseTitle = GameObject.Find("TextPauseTitle").GetComponent<TMP_Text>();
        buttonResumeGame = GameObject.Find("ButtonResumeGame").GetComponent<Button>();
        buttonExitGame = GameObject.Find("ButtonExitGame").GetComponent<Button>();

        textOptionsTitle = GameObject.Find("TextOptionsTitle").GetComponent<TMP_Text>();
        textMasterSlider = GameObject.Find("TextMasterSlider").GetComponent<TMP_Text>();
        textMusicSlider = GameObject.Find("TextMusicSlider").GetComponent<TMP_Text>();
        textSFXSlider = GameObject.Find("TextSFXSlider").GetComponent<TMP_Text>();
        buttonOptionsBack = GameObject.Find("ButtonOptionsBack").GetComponent<Button>();
        sliderMaster = GameObject.Find("SliderMaster").GetComponent<Slider>();
        sliderMusic = GameObject.Find("SliderMusic").GetComponent<Slider>();
        sliderSFX = GameObject.Find("SliderSFX").GetComponent<Slider>();

        textCreditsTitle = GameObject.Find("TextCreditsTitle").GetComponent<TMP_Text>();
        textCreditsBody = GameObject.Find("TextCreditsBody").GetComponent<TMP_Text>();

        textNewGame = GameObject.Find("TextNewGame").GetComponent<TMP_Text>();
        buttonCancelNG = GameObject.Find("ButtonCancelNG").GetComponent<Button>();
        buttonConfirmNG = GameObject.Find("ButtonConfirmNG").GetComponent<Button>();

        playerHitEffect = GameObject.Find("playerHitEffect").GetComponent<Image>();
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
                if (PlayerStats.playerStats == null) return;
                healthSlider.maxValue = PlayerStats.playerStats.maxPlayerHealth;
                suitPower.maxValue = PlayerStats.playerStats.maxSuitPower;

                healthSlider.value = PlayerStats.playerStats.playerHealth;
                suitPower.value = PlayerStats.playerStats.suitPower;
                break;

            case ActiveUI.Pause:
                //if (!InputManager.inputManager.escapePressed)
                //{
                //    SwitchGameplay();
                //}
                break;

            case ActiveUI.Options:
                UpdateOptionsSliderText();
                break;

            case ActiveUI.Credits:
                break;

            case ActiveUI.NewGame:
                break;
        }
    }

    public void UpdateOptionsSliderText()
    {
        textMasterSlider.text = "Master Vol: " + ((int)(sliderMaster.value * 100)).ToString();
        textMusicSlider.text = "Music Vol: " + ((int)(sliderMusic.value * 100)).ToString();
        textSFXSlider.text = "SFX Vol: " + ((int)(sliderSFX.value * 100)).ToString();
    }

    public void SetOptionsSliderValues()
    {
        sliderMaster.value = RandomSoundsManager.RSM.masterVolume;
        sliderSFX.value = RandomSoundsManager.RSM.sfxVolume;
        sliderMusic.value = RandomSoundsManager.RSM.musicVolume;
    }

    public bool OptionsOpen()
    {
        if (activeCanvas == ActiveUI.Options) return true;
        return false;
    }

   //public for use on buttons
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

    public void SwitchMainMenu()
    {
        activeCanvas = ActiveUI.MainMenu;
    }

    public void PlayerHitEffectON(bool isON)
    {
        if (isON) playerHitEffect.enabled = true;
        if (isON == false) playerHitEffect.enabled = false;
    }
}
