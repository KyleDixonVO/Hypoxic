using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnderwaterHorror;

public class MedKit : Item
{
    // Start is called before the first frame update
    [SerializeField] float healthGain = 50f;

    void Start()
    {
        usageTime = 2;
        usageProgress = usageTime;
        itemAudioSource = GetComponent<AudioSource>();
        typeName = "MedKit";
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay || UI_Manager.ui_Manager.PDAOpen())
        {
            AudioManager.audioManager.PauseSound(itemAudioSource);
            return;
        }

        if (beingUsed) // might be able to send to item update()
        {
            TimeToEffect();
        }

        if (FirstPersonController_Sam.fpsSam.carryingHeavyObj) // might be able to send to item update()
        {
            Unequip();
            return;
        }

        if (isEquiped && Input.GetKeyDown(KeyCode.Mouse0) && beingUsed == false)
        {
            AudioManager.audioManager.PlaySound(itemAudioSource, AudioManager.audioManager.syringeUsed);
            beingUsed = true;
            TimeToEffect();
            Debug.Log("click");
        }

        if (gameObject.activeSelf == false)
        {
            AudioManager.audioManager.StopSound(itemAudioSource);
        }
    }

    override protected void ApplyEffect()
    {
        // run code
        PlayerStats.playerStats.playerHealth += healthGain;
       // Debug.LogWarning("works");

        if (PlayerStats.playerStats.playerHealth > PlayerStats.playerStats.maxPlayerHealth)
        {
            PlayerStats.playerStats.playerHealth = PlayerStats.playerStats.maxPlayerHealth;
        }

        // deactivates the object not allowing it to be used
        gameObject.SetActive(false);
    }
}
