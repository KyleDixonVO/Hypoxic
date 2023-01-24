using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats playerStats;

    [Header("Suit Settings")]
    public float maxSuitPower = 100.0f;
    public float suitPower;
    [SerializeField] public float drainPerSecond = 0.67f;
    [SerializeField] public float rechargePerSecond = 2.0f;
    [SerializeField] public float sprintDrainPerSecond = 1.0f;
    [SerializeField] public float drainPerDash = 5.0f;
    [SerializeField] public float suffocationDamage = 1.0f;

    [Header("Player Stats")]
    public float maxPlayerHealth = 100.0f;
    public float playerHealth = 100.0f;
    private bool isDead;

    [Header("Player UI Settings")]
    // Timer for hit effect
    public float startPlayerHitTimer = 0.5f;
    private float playerHitEffectTimer;
    private bool playerHit = false;

    [Header("Spotlight Settings")]
    [SerializeField] public float poweredRange = 60.0f;
    [SerializeField] public float unpoweredRange = 20.0f;
    [SerializeField] public Color poweredColor;
    [SerializeField] public Color unpoweredColor;
    [SerializeField] private Light suitSpotlight;


    private void Awake()
    {
        if (playerStats == null)
        {
            playerStats = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (playerStats != this && playerStats != null)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        suitPower = maxSuitPower;
        playerHealth = maxPlayerHealth;
        playerHitEffectTimer = startPlayerHitTimer;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        FindSpotlightRef();
        ToggleSuitSpotlight();

        ManagePlayerHitEffect();
    }

    public void FindSpotlightRef()
    {
        if (suitSpotlight != null) return;
        suitSpotlight = GameObject.Find("SuitSpotLight").GetComponent<Light>();
    }

    public void SuitDashDrain()
    {
        suitPower -= drainPerDash;
    }

    public void SuitSprintDrain()
    {
        suitPower -= sprintDrainPerSecond * Time.deltaTime;
    }

    public void SuitPowerLogic()
    {
        if (suitPower <= 0)
        {
            suitPower = 0;
            Debug.Log("Suit Power Depleted");
            TakeDamage(suffocationDamage * Time.deltaTime);
            this.gameObject.transform.GetComponentInChildren<Light>().color = unpoweredColor;
            this.gameObject.transform.GetComponentInChildren<Light>().range = unpoweredRange;
        }
        else
        {
            this.gameObject.transform.GetComponentInChildren<Light>().color = poweredColor;
            this.gameObject.transform.GetComponentInChildren<Light>().range = poweredRange;
        }
        suitPower -= drainPerSecond * Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        //if (playerHit == false)
        //{
        //Debug.Log("Taking Damage");
        playerHit = true;
        if (damage <= 0) return;
        Debug.Log("Taking Damage: " + damage);
        playerHealth -= damage;

        if (playerHealth <= 0)
        {
                Debug.Log("Player Health Depleted");
                playerHealth = 0;
                isDead = true;
                if (FirstPersonController_Sam.fpsSam == null) return;
                FirstPersonController_Sam.fpsSam.LockPlayerMovement();
                return;
        }
        //}


    }
    

    public void RechargeSuit()
    {
        if (FirstPersonController_Sam.fpsSam.inWater || suitPower == maxSuitPower) return;
        suitPower += rechargePerSecond * Time.deltaTime;
        if (suitPower > maxSuitPower) suitPower = maxSuitPower;
    }

    public void ToggleSuitSpotlight()
    {
        if (FirstPersonController_Sam.fpsSam.inWater)
        {
            suitSpotlight.enabled = true;
        }
        else
        {
            suitSpotlight.enabled = false;
        }
    }

    public void ManagePlayerHitEffect()
    {
        // Shows the player hit effect on screen for a set amount of time
        if (UI_Manager.ui_Manager == null) return;
        if (playerHit)
        {
            UI_Manager.ui_Manager.PlayerHitEffectON(true);
            playerHitEffectTimer -= Time.deltaTime;
            if (playerHitEffectTimer <= 0)
            {
                UI_Manager.ui_Manager.PlayerHitEffectON(false);
                playerHit = false;
                playerHitEffectTimer = startPlayerHitTimer;
            }
        }

    }
    
    public void ResetRun()
    {
        playerHealth = maxPlayerHealth;
        suitPower = maxSuitPower;
        isDead = false;
    }

    public void LoadPlayerStats()
    {
        Data_Manager.dataManager.UpdatePlayerStats();
    }

    public bool IsDead()
    {
        return isDead;
    }
}
