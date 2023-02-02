using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnderwaterHorror;

public class Glowstick : Item
{
    [SerializeField] float strength = 12f;
    [SerializeField] Light light;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        light.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //TEMP
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isEquiped = true;
            gameObject.GetComponent<Renderer>().enabled = true;
            light.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) | Input.GetKeyDown(KeyCode.Alpha3))
        {
            isEquiped = false;
            gameObject.GetComponent<Renderer>().enabled = false;
            light.enabled = false;
        }

        if (beingUsed)
        {
            TimeToEffect();
        }

        if (FirstPersonController_Sam.fpsSam.carryingHeavyObj)
        {
            Unequip();
            light.enabled = false;
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
        this.gameObject.transform.SetParent(null);
        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        Vector3 forwardForce = strength * transform.forward;
        rigidbody.mass = 1f;       
        rigidbody.AddForce(forwardForce.x, forwardForce.y + 2f, forwardForce.z, ForceMode.Impulse);      
    }
}
