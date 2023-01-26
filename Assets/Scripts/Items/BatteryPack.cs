using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class BatteryPack : Item
    {
        [Header("stats")]
        [SerializeField]
        float powerGain = 30f;

        void Start()
        {
            usageTime = 2;
            usageProgress = usageTime;
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Equip();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                Unequip();
            }

            if (beingUsed)
            {
                TimeToEffect();
            }

            if (FirstPersonController_Sam.fpsSam.carryingHeavyObj)
            {
                Unequip();
                return;
            }

            if (isEquiped && Input.GetKeyDown(KeyCode.Mouse0))
            {
                beingUsed = true;
                TimeToEffect();
            }
        }

        protected override void ApplyEffect()
        {      
            PlayerStats.playerStats.suitPower += powerGain;
            Debug.LogWarning("works");

            if (PlayerStats.playerStats.suitPower > PlayerStats.playerStats.maxSuitPower)
            {
                PlayerStats.playerStats.suitPower = PlayerStats.playerStats.maxSuitPower;
            }

            // deactivates the object not allowing it to be used
            gameObject.SetActive(false);
        }
    }

}
