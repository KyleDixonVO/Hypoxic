using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{

    public static UI_Manager ui_Manager;
    [SerializeField] private Slider suitPower;
    [SerializeField] private Slider healthSlider;


    private void Awake()
    {
        if (ui_Manager == null)
        {
            ui_Manager = this;
        }
        else if (ui_Manager != null && ui_Manager != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        healthSlider.maxValue = FirstPersonController_Sam.fpsSam.maxPlayerHealth;
        healthSlider.value = healthSlider.maxValue;
        suitPower.maxValue = FirstPersonController_Sam.fpsSam.maxSuitPower;
        suitPower.value = suitPower.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = FirstPersonController_Sam.fpsSam.playerHealth;
        suitPower.value = FirstPersonController_Sam.fpsSam.suitPower;
    }
}
