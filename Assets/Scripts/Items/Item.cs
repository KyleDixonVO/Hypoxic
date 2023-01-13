using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Usage Timers")]
    [SerializeField]
    float usageTime;
    [SerializeField]
    float usageProgress;

    [Header("bools")]
    [SerializeField]
    bool isEquiped;

    protected void TimeToEffect()
    {
        if (usageProgress <= 0)
        {
            usageProgress = 0;
            // apply effect
            ApplyEffect();
        }
        else if (usageProgress > 0 && isEquiped)
        {
            usageProgress -= Time.deltaTime;
        }
    }

    virtual protected void ApplyEffect()
    {

    }
}
