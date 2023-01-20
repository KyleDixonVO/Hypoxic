using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Usage Timers")]
    [SerializeField]
    protected float usageTime;
    [SerializeField]
    protected float usageProgress;

    [Header("Refs")]
    [SerializeField]
    protected GameObject playerStats;
    [SerializeField]
    protected PlayerStats PS;

    [Header("bools")]
    [SerializeField]
    protected bool isEquiped;
    [SerializeField]
    protected bool beingUsed;

    private void Start()
    {

    }

    protected void TimeToEffect()
    {      
        if (usageProgress <= 0) // Apply effect
        {
            usageProgress = 0;
            // activets ApplyEffect()
            ApplyEffect();
            beingUsed = false;
        }
        else if (usageProgress > 0 && isEquiped) // Timer
        {
            usageProgress -= Time.deltaTime;
        }
        else if (!isEquiped) // stop timer if unequiped
        {
            beingUsed = false;
            usageProgress = usageTime;
        }
    }

    virtual protected void ApplyEffect()
    {

    }
    protected void Unequip()
    {
        isEquiped = false;
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    protected void Equip()
    {
        isEquiped = true;
        gameObject.GetComponent<Renderer>().enabled = true;
    }
}
