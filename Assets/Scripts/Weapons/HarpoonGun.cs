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
            reloadTime = 2;

            // set bools
            canShoot = true;
            isEquiped = false;

            // Tobias's Polymorph audio
            weaponAudioSource = this.gameObject.GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
            // RELOAD COUTER
            if (reloadProgress <= 0)
            {
                AudioManager.audioManager.StopSound(weaponAudioSource);
                reloadProgress = 0;
                canShoot = true;
            }
            else if (reloadProgress > 0 && isEquiped)
            {
                AudioManager.audioManager.playSound(weaponAudioSource, AudioManager.audioManager.harpoonReloading); // - Tobias Audio
                reloadProgress -= Time.deltaTime;
            }

            // SHOOTING
            if (Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo > 0 && canShoot && isEquiped)
            {
                AudioManager.audioManager.playSound(weaponAudioSource, AudioManager.audioManager.harpoonShot); // - Tobias Audio
                ShootWeapon(damage);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo <= 0 && isEquiped)
            {
                // no ammo sound
                AudioManager.audioManager.playSound(weaponAudioSource, AudioManager.audioManager.harpoonNoAmmo); // - Tobias Audio
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
