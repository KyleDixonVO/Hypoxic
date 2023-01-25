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
    }

    // Update is called once per frame
    void Update()
    {
        // if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Equip();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            Unequip();
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

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            beingUsed = true;
            TimeToEffect();
            Debug.Log("click");
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
