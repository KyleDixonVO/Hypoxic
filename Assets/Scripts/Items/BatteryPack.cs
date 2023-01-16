using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPack : Item
{
    [Header("stats")]
    [SerializeField]
    float powerGain = 30f;

    void Start()
    {
        // set refs
        playerStats = GameObject.Find("Player(w/items)");
        PS = playerStats.GetComponent<PlayerStats>();

        usageTime = 2;
        usageProgress = usageTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            isEquiped = true;
            gameObject.GetComponent<Renderer>().enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            isEquiped = false;
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        if (isEquiped && Input.GetKeyDown(KeyCode.Mouse0))
        {
            beingUsed = true;
            TimeToEffect();
        }

        if (beingUsed)
        {
            TimeToEffect();
        }
    }

    protected override void ApplyEffect()
    {      
        PS.suitPower += powerGain;
        Debug.LogWarning("works");

        if (PS.suitPower > PS.maxSuitPower)
        {
            PS.suitPower = PS.maxSuitPower;
        }

        // deactivates the object not allowing it to be used
        gameObject.SetActive(false);
    }
}
