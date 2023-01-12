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
    [Header("Booleans")]
    public bool isEquiped;

    // We might want to put items like this on a diffrent rendering plane / canvas to prevent
    // clipping with the enviroment - Edmund

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo > 0)
        {
            ShootHarpoon();
            // harpoon sound
            if (currentAmmo != 0)
            {
                currentAmmo--;
            }           
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && currentAmmo <= 0)
        {
            // no ammo sound
        }

        // update HUD
    }

    void ShootHarpoon()
    {
        Debug.LogWarning("BANG");
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * range, Color.red);
            if (hit.transform.tag == "Enemy")
            {
                // run ai behavior / kill ai
            }
            else
            {
                Debug.LogWarning("hit:" + hit);
            }
        }
    }
}
