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
        public bool pdaOpen;

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
        [SerializeField] private Image vignetteEffect;      
        [SerializeField] private TMP_Text textToolTipE;
        [SerializeField] private TMP_Text textToolTipR;
        [SerializeField] private TMP_Text textObjectives;
        [SerializeField] private TMP_Text textAmmoCounter;
        [SerializeField] public Animator fade;
        [SerializeField] private Slider sliderRepair;

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

        [Header("PDA Assets")]
        [SerializeField] private Button buttonInventory;
        [SerializeField] private Button buttonObjectives;
        [SerializeField] private Button buttonLogs;
        [SerializeField] private GameObject pdaObjectiveParent;
        [SerializeField] private TMP_Text textPDAObjectives;
        [SerializeField] private GameObject pdaInventoryParent;
        [SerializeField] private GameObject pdaLogParent;
        [SerializeField] private TMP_Text textLogTranscript;
        [SerializeField] private Image[] playerInventoryImages;
        [SerializeField] private Sprite[] itemImages;
        [SerializeField] private string[] logTranscripts;
        [SerializeField] private PDASubmenu submenu;
        public InventoryButton[] inventoryButtons;

        [SerializeField] private AudioSource logSource;



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

        public enum PDASubmenu 
        { 
            Inventory,
            Objectives,
            Logs
        }

        public enum Log
        {
            Log1,
            Log2,
            Log3,
            Log4,
            Log5,
            Log6
        }

        public enum Items
        {
            Gun,
            Prod,
            Battery,
            Medkit,
            Glowstick,
            NoItem

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
            vignetteEffect = GameObject.Find("VignetteEffect").GetComponent<Image>();
            textToolTipE = GameObject.Find("TextToolTipE").GetComponent<TMP_Text>();
            textToolTipR = GameObject.Find("TextToolTipR").GetComponent<TMP_Text>();
            sliderRepair = GameObject.Find("SliderRepair").GetComponent<Slider>();
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

            buttonInventory = GameObject.Find("ButtonInventory").GetComponent<Button>();
            buttonObjectives = GameObject.Find("ButtonObjectives").GetComponent<Button>();
            buttonLogs = GameObject.Find("ButtonLogs").GetComponent<Button>();
            pdaInventoryParent = GameObject.Find("PDAInventoryParent");
            pdaLogParent = GameObject.Find("PDALogParent");
            pdaObjectiveParent = GameObject.Find("PDAObjectiveParent");
            textPDAObjectives = GameObject.Find("TextPDAObjectives").GetComponent<TMP_Text>();
            textLogTranscript = GameObject.Find("TextLogTranscript").GetComponent<TMP_Text>();

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
                    TogglePDASubmenus();
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
                    UpdateRepairSliderValue();
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

        //Toggles for buttons on the main menu canvas
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

        //PDA methods
        void TogglePDASubmenus()
        {
            PDASubmenu passthrough = PDASubmenu.Inventory;
            if (SubmenuButton.caller != null) 
            {
                passthrough = SubmenuButton.caller.activeSubmenu;
            }
            
            switch (passthrough) 
            {
                case PDASubmenu.Inventory:
                    UpdatePDAInventoryImages();
                    SetActiveSlotInPDA();
                    if (submenu == passthrough) return;
                    pdaInventoryParent.SetActive(true);
                    pdaLogParent.SetActive(false);
                    pdaObjectiveParent.SetActive(false);
                    break;

                case PDASubmenu.Logs:
                    if (submenu == passthrough) return;
                    pdaInventoryParent.SetActive(false);
                    pdaLogParent.SetActive(true);
                    pdaObjectiveParent.SetActive(false);
                    break;

                case PDASubmenu.Objectives:
                    UpdatePDAObjectiveText();
                    if (submenu == passthrough) return;
                    pdaInventoryParent.SetActive(false);
                    pdaLogParent.SetActive(false);
                    pdaObjectiveParent.SetActive(true);
                    break;
            }
            submenu = passthrough;
        }

        void SetActiveSlotInPDA()
        {
            if (canvasArray[(int)ActiveUI.PDA].enabled == false) return;
            if (InventoryButton.caller == null) return;
            Debug.LogWarning("PDA open, setting lastNumPressed to: " + InventoryButton.caller.slot);
            InputManager.inputManager.SetLastNumPressed(InventoryButton.caller.slot);
        }


        void UpdatePDAInventoryImages()
        {
            for (int i = 0; i < PlayerInventory.playerInventory.inventory.Length; i++)
            {
                if (PlayerInventory.playerInventory.inventory[i] == null)
                {
                    Debug.LogWarning("No item in inventory slot");
                    playerInventoryImages[i].sprite = itemImages[(int)Items.NoItem];
                    continue;
                }

                switch (PlayerInventory.playerInventory.inventory[i].GetComponent<Interactable>().typeName)
                {
                    case "BatteryPack":
                        playerInventoryImages[i].sprite = itemImages[(int)Items.Battery];
                        break;

                    case "MedKit":
                        playerInventoryImages[i].sprite = itemImages[(int)Items.Medkit];
                        break;

                    case "Glowstick":
                        playerInventoryImages[i].sprite = itemImages[(int)Items.Glowstick];
                        break;

                    case "ElectroProd":
                        playerInventoryImages[i].sprite = itemImages[(int)Items.Prod];
                        break;

                    case "HarpoonGun":
                        playerInventoryImages[i].sprite = itemImages[(int)Items.Gun];
                        break;

                    default:
                        Debug.LogWarning("typeName cannot be read");
                        Debug.LogWarning(PlayerInventory.playerInventory.inventory[i].GetComponent<Interactable>().typeName);
                        playerInventoryImages[i].sprite = itemImages[(int)Items.NoItem];
                        break;
                }
            }
        }

        void UpdatePDAObjectiveText()
        {
            textPDAObjectives.text = Objective_Manager.objective_Manager.AssignObjectiveText();
        }

        public void UpdateLogSubmenu()
        {
            textLogTranscript.text = logTranscripts[(int)LogButton.caller.activeLog];

            if (AudioManager.audioManager.audioLogs[(int)LogButton.caller.activeLog] == null) return;
            AudioManager.audioManager.PlaySound(logSource, AudioManager.audioManager.audioLogs[(int)LogButton.caller.activeLog]);
        }

        //Updates the camera reference on objects that use post processing
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

        //Updates ammo count, as well as health and suit power slider values
        void UpdateGameplayHUD()
        {
            textObjectives.text = Objective_Manager.objective_Manager.AssignObjectiveText();
            if (PlayerInventory.playerInventory.inventory[PlayerInventory.playerInventory.activeWeapon] != null && PlayerInventory.playerInventory.inventory[PlayerInventory.playerInventory.activeWeapon].GetComponent<Weapon>())
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

        //Updating various slider values
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

        public void UpdateRepairSliderValue()
        {   
            if(FirstPersonController_Sam.fpsSam.GetComponentInChildren<RepairObject>() == null)
            {
                DisableSecondaryInteractText();
                sliderRepair.gameObject.SetActive(false);
            }
            else if (FirstPersonController_Sam.fpsSam.GetComponentInChildren<RepairObject>().repairing)
            {
                sliderRepair.gameObject.SetActive(true);
                sliderRepair.value = FirstPersonController_Sam.fpsSam.GetComponentInChildren<RepairObject>().repairPercentage;
                if (Enemy_Manager.enemy_Manager.enemies[0].GetComponent<BigEnemyFish>() != null)
                {
                    Enemy_Manager.enemy_Manager.enemies[0].GetComponent<BigEnemyFish>().CallBigFish();
                }
            }
            else
            {
                sliderRepair.gameObject.SetActive(false);
            }
        }

        //Bools to check for various UI states elsewhere in code
        public bool OptionsOpen()
        {
            if (activeCanvas == ActiveUI.Options) return true;
            return false;
        }

        public bool PDAOpen()
        {
            if (canvasArray[(int)ActiveUI.PDA].enabled) return true;
            return false;
        }

        //Toggles for interaction UI text
        public void ActivatePrimaryInteractText(string tooltip)
        {
            textToolTipE.text = tooltip;
            textToolTipE.gameObject.SetActive(true);
        }

        public void DisablePrimaryInteractText()
        {
            textToolTipE.gameObject.SetActive(false);
        }

        public void ActivateSecondaryInteractText(string tooltip)
        {
            textToolTipR.text = tooltip;
            textToolTipR.gameObject.SetActive(true);
        }

        public void DisableSecondaryInteractText()
        {
            textToolTipR.gameObject.SetActive(false);
        }

        // Scene Transition
        public void FadeOut()
        {
            fade.SetTrigger("FadeOut");
        }
        public void FadeIn()
        {
            fade.SetTrigger("FadeIn");
        }

        public void OnFadeComplete()
        {          
            if (Level_Manager.LM.IsSceneOpen("Outside")) Level_Manager.LM.LoadMainHab();
            else if (Level_Manager.LM.IsSceneOpen("DemoBuildingInside")) Level_Manager.LM.LoadOutside();
            FadeIn();
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

        public void ResetForNewRun()
        {
            SubmenuButton.caller = null;
            LogButton.caller = null;
        }

        public void VignetteEffectOn(bool isOn)
        {
            if (isOn) LeanTween.value(vignetteEffect.gameObject, vignetteEffect.color = new Color(0, 0, 0, 225), vignetteEffect.color = new Color(0, 0, 0, 0), 0.5f);
            else LeanTween.value(vignetteEffect.gameObject, vignetteEffect.color = new Color(0, 0, 0, 0), vignetteEffect.color = new Color(0, 0, 0, 225), 0.5f);
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
