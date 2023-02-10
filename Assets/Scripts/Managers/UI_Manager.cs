using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UnderwaterHorror
{
    public class UI_Manager : MonoBehaviour
    {
        public static UI_Manager ui_Manager;
        public bool tooltipActiveElsewhere = false;

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
        [SerializeField] private Image playerDrownEffect;
        [SerializeField] private TMP_Text textToolTipE;
        [SerializeField] private TMP_Text textToolTipR;
        [SerializeField] private TMP_Text textObjectives;
        [SerializeField] private TMP_Text textAmmoCounter;

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

        [Header("Game Over / Win Assets")]
        [SerializeField] private TMP_Text textGameOver;
        [SerializeField] private Button buttonClearGameOver;

        [Header("General Variables")]
        [SerializeField] private ActiveUI activeCanvas;
        [SerializeField] private Canvas[] canvasArray;
        [SerializeField] private Camera mainMenuCam;
        [SerializeField] private Camera gameplayCam;


        public enum ActiveUI
        {
            MainMenu,
            Gameplay,
            Pause,
            Options,
            Credits,
            NewGame,
            GameOverWin,
            PDA
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
            DisableSecondaryInteractText();
            DisablePrimaryInteractText();
        }

        // Update is called once per frame
        void Update()
        {
            CheckActiveCanvas();
            FindCameraRefs();
            UpdatePostProcessCamRef();
        }

        void FindReferences()
        {
            
            textStartTitle = GameObject.Find("TextStartTitle").GetComponent<TMP_Text>();
            buttonStartGame = GameObject.Find("ButtonStartGame").GetComponent<Button>();
            buttonNewGame = GameObject.Find("ButtonNewGame").GetComponent<Button>();
            buttonOptions = GameObject.Find("ButtonOptions").GetComponent<Button>();
            buttonExit = GameObject.Find("ButtonExit").GetComponent<Button>();

            playerHitEffect = GameObject.Find("PlayerHitEffect").GetComponent<Image>();
            textToolTipE = GameObject.Find("TextToolTipE").GetComponent<TMP_Text>();
            textToolTipR = GameObject.Find("TextToolTipR").GetComponent<TMP_Text>();
            textObjectives = GameObject.Find("TextObjectives").GetComponent<TMP_Text>();
            textAmmoCounter = GameObject.Find("TextAmmoCounter").GetComponent<TMP_Text>();

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

            textGameOver = GameObject.Find("TextGameOver").GetComponent<TMP_Text>();
            buttonClearGameOver = GameObject.Find("ButtonClearGameOver").GetComponent<Button>();
        }

        void FindCameraRefs()
        {
            if (FirstPersonController_Sam.fpsSam != null)
            {
                gameplayCam = GameObject.Find("Main Camera").GetComponent<Camera>();
            }
            if (activeCanvas == ActiveUI.MainMenu)
            {
                mainMenuCam = GameObject.Find("MainMenuCamera").GetComponent<Camera>();
            }
        }

        void CheckActiveCanvas()
        {
            for (int i = 0; i < canvasArray.Length; i++)
            {
                if (i == (int)activeCanvas)
                {
                    canvasArray[i].enabled = true;
                    continue;
                }

                if (activeCanvas == ActiveUI.Pause && i == (int)ActiveUI.Gameplay) continue;
                else if (activeCanvas == ActiveUI.Gameplay && InputManager.inputManager.tabPressed && i == (int)ActiveUI.PDA)
                {
                    canvasArray[i].enabled = true;
                    continue;
                }
                else if (activeCanvas == ActiveUI.NewGame&& i == (int)ActiveUI.MainMenu) continue;
                else
                {
                    canvasArray[i].enabled = false;
                }
            }

            switch (activeCanvas)
            {
                case ActiveUI.MainMenu:
                    UpdatePostProcessCamRef();
                    EnableMainMenuButtons();
                    ToggleLoadGameButton();
                    
                    break;

                case ActiveUI.Gameplay:
                    UpdateGameplayHUD();
                    break;

                case ActiveUI.Pause:

                    break;

                case ActiveUI.Options:
                    UpdateOptionsSliderText();
                    break;

                case ActiveUI.Credits:
                    break;

                case ActiveUI.NewGame:
                    DisableMainMenuButtons();
                    break;

            }
        }

        void DisableMainMenuButtons()
        {
            buttonStartGame.interactable = false;
            buttonNewGame.interactable = false;
            buttonOptions.interactable = false;
            buttonExit.interactable = false;
        }

        void EnableMainMenuButtons()
        {
            buttonStartGame.interactable = true;
            buttonNewGame.interactable = true;
            buttonOptions.interactable = true;
            buttonExit.interactable = true;
        }

        void ToggleLoadGameButton()
        {
            
            if (!Data_Manager.dataManager.SaveExists())
            {
                if (!buttonStartGame.interactable) return;
                buttonStartGame.interactable = false;
            }
            else if (Data_Manager.dataManager.SaveExists())
            {
                if (buttonStartGame.interactable) return;
                buttonStartGame.interactable = true;
            }
        }

        void UpdatePostProcessCamRef()
        {
            for (int i = 0; i < canvasArray.Length; i++)
            {
                if (activeCanvas == ActiveUI.MainMenu)
                {
                    if (canvasArray[i].worldCamera == mainMenuCam) continue;
                    canvasArray[i].worldCamera = mainMenuCam;
                }
                else if (activeCanvas == ActiveUI.Gameplay)
                {
                    if (canvasArray[i].worldCamera == gameplayCam) continue;
                    canvasArray[i].worldCamera = gameplayCam;
                }
                
            }
        }

        void UpdateGameplayHUD()
        {
            textObjectives.text = Objective_Manager.objective_Manager.AssignObjectiveText();
            if (PlayerInventory.playerInventory.inventory[PlayerInventory.playerInventory.activeWeapon].GetComponent<Weapon>() != null)
            {
                textAmmoCounter.text = PlayerInventory.playerInventory.inventory[PlayerInventory.playerInventory.activeWeapon].GetComponent<Weapon>().currentAmmo +
                               " / " + PlayerInventory.playerInventory.inventory[PlayerInventory.playerInventory.activeWeapon].GetComponent<Weapon>().reserves;
            }

            if (PlayerStats.playerStats == null) return;
            healthSlider.maxValue = PlayerStats.playerStats.maxPlayerHealth;
            suitPower.maxValue = PlayerStats.playerStats.maxSuitPower;

            healthSlider.value = PlayerStats.playerStats.playerHealth;
            suitPower.value = PlayerStats.playerStats.suitPower;
        }

        public void UpdateOptionsSliderText()
        {
            textMasterSlider.text = "Master Vol: " + ((int)(sliderMaster.value * 100)).ToString();
            textMusicSlider.text = "Music Vol: " + ((int)(sliderMusic.value * 100)).ToString();
            textSFXSlider.text = "SFX Vol: " + ((int)(sliderSFX.value * 100)).ToString();
        }

        public void SetOptionsSliderValues()
        {
            sliderMaster.value = AudioManager.audioManager.masterVolume;
            sliderSFX.value = AudioManager.audioManager.sfxVolume;
            sliderMusic.value = AudioManager.audioManager.musicVolume;
        }

        public bool OptionsOpen()
        {
            if (activeCanvas == ActiveUI.Options) return true;
            return false;
        }

        public void ActivatePrimaryInteractText()
        {
            textToolTipE.gameObject.SetActive(true);
        }

        public void DisablePrimaryInteractText()
        {
            textToolTipE.gameObject.SetActive(false);
        }

        public void ActivateSecondaryInteractText()
        {
            textToolTipR.gameObject.SetActive(true);
        }

        public void DisableSecondaryInteractText()
        {
            textToolTipR.gameObject.SetActive(false);
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
    
        public void SwitchGameOverWin()
        {
            activeCanvas = ActiveUI.GameOverWin;
        }

        public void SwitchDeathText()
        {
            textGameOver.text = "Game Over";
        }

        public void SwitchWinText()
        {
            textGameOver.text = "You Won!";
        }

        //  Tobias's Amazing Code Powers
        public void PlayerHitEffectON(bool isON)
        {
            if (isON) playerHitEffect.enabled = true;
            if (isON == false) playerHitEffect.enabled = false;
        }

        public void PlayerDrownEffectON(bool isON)
        {
            if (isON) playerDrownEffect.enabled = true;
            if (isON == false) playerDrownEffect.enabled = false;
        }

        public void PlayUIButtonSound()
        {
            AudioManager.audioManager.PlaySound(this.gameObject.GetComponent<AudioSource>(), AudioManager.audioManager.uIButton);
        }
        //---------------------------------------------------------------------------------------------------------------------------
    }

}
