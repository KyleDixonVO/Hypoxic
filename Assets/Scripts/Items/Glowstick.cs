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
        itemAudioSource = GetComponent<AudioSource>();
        typeName = "Glowstick";
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay || UI_Manager.ui_Manager.PDAOpen())
        {
            AudioManager.audioManager.PauseSound(itemAudioSource);
            return;
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

        if (isEquiped && Input.GetKeyDown(KeyCode.Mouse0) && beingUsed == false)
        {
            AudioManager.audioManager.PlaySound(itemAudioSource, AudioManager.audioManager.glowstickUsed);
            light.enabled = true;
            beingUsed = true;
            TimeToEffect();
        }

        if (gameObject.activeSelf == false)
        {
            AudioManager.audioManager.StopSound(itemAudioSource);
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
        gameObject.GetComponent<Collider>().enabled = true;
        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        Vector3 forwardForce = strength * transform.right;
        //rigidbody.mass = 1f;       
        rigidbody.AddForce(forwardForce.x, forwardForce.y + 2f, forwardForce.z, ForceMode.Impulse);      
    }
}
