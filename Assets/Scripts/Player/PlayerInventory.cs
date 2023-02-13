using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderwaterHorror
{
    public class PlayerInventory : MonoBehaviour
    {
        public GameObject[] inventory;
        int inventorySize = 3;
        Camera playerCam;
        public static PlayerInventory playerInventory;
        public int activeWeapon;

        Vector3 itemPos = new Vector3(.35f, -.35f, 1f);
        Quaternion itemRot = Quaternion.Euler(0f, 270f, 0f);

        void Awake()
        {
            if (playerInventory == null)
            {
                playerInventory = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else if (playerInventory != this && playerInventory != null)
            {
                Destroy(this.gameObject);
            }
        }

        void Start()
        {
            inventory = new GameObject[inventorySize];
            playerCam = FirstPersonController_Sam.fpsSam.playerCamera.GetComponent<Camera>();
        }

        void Update()
        {
            HandleEquipUnequip();
            HandleItemUsage();
        }

        // ---------------------------------- Inventory --------------------------------------- \\
        public void AddToInventory(GameObject itemToAdd)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == null)
                {
                    itemToAdd.transform.SetParent(FirstPersonController_Sam.fpsSam.playerCamera.transform);
                    itemToAdd.transform.localRotation = itemRot;
                    itemToAdd.transform.localPosition = itemPos;
                    itemToAdd.layer = 0;
                    if (itemToAdd.GetComponent<Weapon>()) itemToAdd.GetComponent<Weapon>().playerCamera = playerCam;

                    inventory[i] = itemToAdd;

                    for (int j = 0; j < inventory.Length; j++)
                    {
                        if (j == i) Equip(inventory[j], j);
                        else Unequip(inventory[j]);
                    }
                }
            }
        }

        // ------------------------------- Update Loop ------------------------------------------ \\
        void HandleEquipUnequip()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && inventory[0] != null)
            {
                Equip(inventory[0], 0);
                Unequip(inventory[1]);
                Unequip(inventory[2]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && inventory[1] != null)
            {
                Equip(inventory[1], 1);
                Unequip(inventory[0]);
                Unequip(inventory[2]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && inventory[2] != null)
            {
                Equip(inventory[2], 2);
                Unequip(inventory[1]);
                Unequip(inventory[0]);
            }
        }

        void HandleItemUsage()
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] != null && inventory[i].GetComponent<Item>() && inventory[i].GetComponent<Item>().isUsed)
                {
                    inventory[i] = null;
                }
            }
        }

        // ------------------------------------ Equip / Unequip ------------------------------------------------ \\
        void Equip(GameObject item, int slot)
        {
            if (item.GetComponent<Glowstick>()) item.GetComponent<Glowstick>().TurnOn(); // enable light

            if (item.GetComponent<Weapon>()) item.GetComponent<Weapon>().isEquiped = true;
            else item.GetComponent<Item>().isEquiped = true;
            item.gameObject.GetComponent<Renderer>().enabled = true;

            if (item.GetComponent<Weapon>()) activeWeapon = slot;
        }

        void Unequip(GameObject item)
        {
            if (item.GetComponent<Glowstick>()) item.GetComponent<Glowstick>().TurnOff(); // disable light

            if (item.GetComponent<Weapon>()) item.GetComponent<Weapon>().isEquiped = false;
            else item.GetComponent<Item>().isEquiped = false;
            item.gameObject.GetComponent<Renderer>().enabled = false;
        }
    }
}
