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

    public void TurnOn()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
    }
    public void TurnOff()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    protected override void ApplyEffect()
    {
        this.gameObject.transform.SetParent(null);
        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        Vector3 forwardForce = strength * transform.right;
        rigidbody.mass = 1f;       
        rigidbody.AddForce(forwardForce.x, forwardForce.y + 2f, forwardForce.z, ForceMode.Impulse);      
    }
}
