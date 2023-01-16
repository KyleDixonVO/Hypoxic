using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroProd : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        // set weapon stats
        damage = 8;
        range = 2;
        maxAmmo = 3;
        currentAmmo = 3;
        reloadTime = 2;

        // set bools
        canShoot = true;
        isEquiped = false;
    }

    // Update is called once per frame
    void Update()
    {
        // RELOAD COUTER
        if (reloadProgress <= 0)
        {
            reloadProgress = 0;
            canShoot = true;
        }
        else if (reloadProgress > 0 && isEquiped)
        {
            reloadProgress -= Time.deltaTime;
        }

        // SHOOTING
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo > 0 && canShoot && isEquiped)
        {
            ShootWeapon(damage);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo <= 0 && isEquiped)
        {
            // no ammo sound
        }

        //TEMP
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isEquiped = true;
            gameObject.GetComponent<Renderer>().enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) | Input.GetKeyDown(KeyCode.Alpha3))
        {
            isEquiped = false;
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //EquipUnequip();
        }
    }
}
