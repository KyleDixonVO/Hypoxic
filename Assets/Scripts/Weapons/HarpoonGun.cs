using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class HarpoonGun : Weapon
    {
        // We might want to put items like this on a diffrent rendering plane / canvas to prevent
        // clipping with the enviroment - Edmund

        // Start is called before the first frame update
        void Start()
        {
            // set weapon stats
            damage = 5;
            range = 15;
            maxAmmo = 6;
            currentAmmo = 6;
            reloadTime = 5;

            // set bools
            canShoot = true;
            isEquiped = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
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

            if (FirstPersonController_Sam.fpsSam.carryingHeavyObj)
            {
                Unequip();
                return;
            }

            //TEMP
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                isEquiped = true;
                gameObject.GetComponent<Renderer>().enabled = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) | Input.GetKeyDown(KeyCode.Alpha3))
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

}
