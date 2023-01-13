using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonGun : MonoBehaviour
{
    [Header("Refrences")]
    public Camera playerCamera;
    [Header("Weapon Values")]
    [SerializeField]
    float range;
    [SerializeField]
    int currentAmmo;
    [SerializeField]
    int maxAmmo;
    [SerializeField]
    float reloadTime = 5f;
    [SerializeField]
    float reloadProgress;
    [Header("Booleans")]
    public bool isEquiped;
    bool canShoot;

    // We might want to put items like this on a diffrent rendering plane / canvas to prevent
    // clipping with the enviroment - Edmund

    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
        isEquiped = true;
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
            ShootHarpoon();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo <= 0 && isEquiped)
        {
            // no ammo sound
        }

        // TEMP PUT ITEM AWAY
        if (Input.GetKeyDown(KeyCode.Q))
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
        }

        // update HUD
       // Debug.LogWarning("time to reload: " + reloadProgress);
    }

    void ShootHarpoon()
    {
        Debug.LogWarning("BANG");
        canShoot = false;
        reloadProgress = reloadTime;

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            // harpoon sound
            currentAmmo--;

            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * range, Color.red);

            if (hit.transform.gameObject.layer == 8) // layer 8 == Enemy
            {
                Debug.LogWarning("hit enemy");
                EnemyStats stats = hit.transform.GetComponent<EnemyStats>();
                stats.health = 0;
            }
        }
    }

    public void GetAmmo(int ammoAmmount)
    {
        // range checks
        if (ammoAmmount > 6) ammoAmmount = 6;
        else if (ammoAmmount <= 0) return;

        // get ammo
        currentAmmo += ammoAmmount;
        if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
    }
}
