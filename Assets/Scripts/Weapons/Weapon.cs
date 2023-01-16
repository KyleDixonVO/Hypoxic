using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Refrences")]
    public Camera playerCamera;
    [Header("Weapon Values")]
    [SerializeField]
    protected float range;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected int currentAmmo;
    [SerializeField]
    protected int maxAmmo;
    [SerializeField]
    protected float reloadTime = 5f;
    [SerializeField]
    protected float reloadProgress;
    [Header("Booleans")]
    public bool isEquiped;
    protected bool canShoot;


    void Update()
    {
        // update HUD
        // Debug.LogWarning("time to reload: " + reloadProgress);
    }

    protected void ShootWeapon(int damage)
    {
        Debug.LogWarning("BANG");
        canShoot = false;
        reloadProgress = reloadTime;

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            // harpoon sound
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * range, Color.red);

            if (hit.transform.gameObject.layer == 8) // layer 8 == Enemy
            {
                Debug.LogWarning("hit enemy");
                EnemyStats stats = hit.transform.GetComponent<EnemyStats>();
                stats.health -= damage;
            }
        }
        currentAmmo--;
    }

    /*protected void EquipUnequip() // TEMP PUT ITEM AWAY
    {
        if (isEquiped)
        {
            isEquiped = false;
            gameObject.GetComponent<Renderer>().enabled = false;
            if (reloadProgress != 0) reloadProgress = reloadTime;
        }
        else if (!isEquiped)
        {
            isEquiped = true;
            gameObject.GetComponent<Renderer>().enabled = true;
        }
    }*/

    public void GetAmmo(int ammoAmmount)
    {
        // range checks
        if (ammoAmmount > maxAmmo) ammoAmmount = maxAmmo;
        else if (ammoAmmount <= 0) return;

        // get ammo
        currentAmmo += ammoAmmount;
        if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
    }
}
