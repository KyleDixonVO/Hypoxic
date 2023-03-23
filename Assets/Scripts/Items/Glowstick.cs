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

    public override void ResetForNewRun()
    {
        base.ResetForNewRun();
        light.enabled = false;
    }

    public void TurnOn()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
        light.enabled = true;
    }
    public void TurnOff()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        light.enabled = false;
    }

    protected override void ApplyEffect()
    {
        if (gameObject.GetComponent<Rigidbody>() != null) return;
        this.gameObject.transform.SetParent(null);
        gameObject.GetComponent<Collider>().enabled = true;

        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        Vector3 forwardForce = strength * transform.right;
        //rigidbody.mass = 1f;       
        GetComponent<Rigidbody>().AddForce(forwardForce.x, forwardForce.y + 2f, forwardForce.z, ForceMode.Impulse);
        isEquiped = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
       if (GetComponent<Rigidbody>() != null && GetComponent<Rigidbody>().velocity.normalized == Vector3.zero && collision.transform.gameObject.layer == 7)
       {
            Debug.Log("hit");
            Destroy(GetComponent<Rigidbody>());
       }
    }
}
