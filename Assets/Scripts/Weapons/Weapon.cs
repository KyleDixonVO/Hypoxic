using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnderwaterHorror;

namespace UnderwaterHorror
{
    public class Weapon : Interactable
    {
        [Header("Refrences")]
        public Camera playerCamera;
        [Header("Weapon Values")]
        [SerializeField]
        protected float range;
        [SerializeField]
        protected int damage;
        [SerializeField]
        public int currentAmmo;
        public int reserves;
        [SerializeField]
        protected int maxAmmo;
        [SerializeField]
        protected float reloadTime = 5f;
        [SerializeField]
        protected float reloadProgress;
        [Header("Booleans")]
        public bool isEquiped;
        protected bool canShoot;
        [Header("SFX")]
        [SerializeField] GameObject bloodSFX;
        [Header("sprite")]
        public Sprite icon;

        // Sound Polymorphism
        protected AudioSource weaponAudioSource;

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
                    stats.TakeDamage(damage);

                    GameObject blood = Instantiate(bloodSFX);
                    blood.transform.position = hit.point;
                    StartCoroutine(waitTime(blood));
                }

                Debug.LogWarning("hit " + hit);
            }
            currentAmmo--;
        }

        public void GetAmmo(int ammoAmmount)
        {
            // range checks
            if (ammoAmmount > maxAmmo) ammoAmmount = maxAmmo;
            else if (ammoAmmount <= 0) return;

            // get ammo
            currentAmmo += ammoAmmount;
            if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
        }

        protected void Unequip()
        {
            isEquiped = false;
            gameObject.GetComponent<Renderer>().enabled = false;
        }

        protected void Equip()
        {
            isEquiped = true;
            gameObject.GetComponent<Renderer>().enabled = true;
        }

        IEnumerator waitTime(GameObject sfx)
        {
            yield return new WaitForSeconds(2f);
            Destroy(sfx);
        }

        public override void OnInteract()
        {
            PlayerInventory.playerInventory.AddToInventory(this.gameObject);
            // throw new System.NotImplementedException();
        }

        public override void OnFocus()
        {
            UI_Manager.ui_Manager.ActivatePrimaryInteractText();
           // throw new System.NotImplementedException();
        }

        public override void OnLoseFocus()
        {
            UI_Manager.ui_Manager.DisablePrimaryInteractText();
           // throw new System.NotImplementedException();
        }
    }

}
