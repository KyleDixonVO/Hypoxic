using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnderwaterHorror
{
    public class PlayerInventory : MonoBehaviour
    {
        public GameObject[] inventory;
        public GameObject[] PDAItems;
        int inventorySize = 3;
        Camera playerCam;
        public static PlayerInventory playerInventory;
        public int activeWeapon; // not actually a great name as it can be any item - Edmund
        [SerializeField] Sprite emptySlot;

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
            HandleEquipUnequip(); // Checks for the input to switch to weapons
            HandleItemUsage(); // checks to see if an item is used, deletes it if so
            if (Input.GetKeyDown(KeyCode.Q)) HandleDrop(); // checks the input for droping an item
        }

        // ---------------------------------- Inventory --------------------------------------- \\
        public void AddToInventory(GameObject itemToAdd)
        {
            for (int i = 0; i < inventory.Length; i++) // finds first available inventory slot for item
            {
                if (inventory[i] == null)
                {
                    itemToAdd.transform.SetParent(FirstPersonController_Sam.fpsSam.playerCamera.transform);
                    itemToAdd.transform.localRotation = itemRot;
                    itemToAdd.transform.localPosition = itemPos;
                    itemToAdd.layer = 0;
                    if (itemToAdd.GetComponent<Rigidbody>()) Destroy(itemToAdd.GetComponent<Rigidbody>());
                    if (itemToAdd.GetComponent<Weapon>())
                    {
                        itemToAdd.GetComponent<Weapon>().playerCamera = playerCam;
                        //PDAItems[i].gameObject.GetComponent<Image>().overrideSprite = itemToAdd.GetComponent<Weapon>().icon;
                    }
                    else
                    {
                        //PDAItems[i].gameObject.GetComponent<Image>().overrideSprite = itemToAdd.GetComponent<Item>().icon;
                    }

                    inventory[i] = itemToAdd;
                    inventory[i].gameObject.GetComponent<Collider>().enabled = false;



                    for (int j = 0; j < inventory.Length; j++) // DONT REMOVE
                    {
                        if (j == i) Equip(inventory[j], j);
                        else Unequip(inventory[j]);
                    }
                    return;
                }
            }
        }

        // ------------------------------- Update Loop ------------------------------------------ \\
        void HandleEquipUnequip() // handles the old equip / unequip system
        {
            activeWeapon = InputManager.inputManager.lastNumKeyPressed;

            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] == null) continue;
                if (FirstPersonController_Sam.fpsSam.carryingHeavyObj)
                {
                    Unequip(inventory[i]);
                    continue;
                }

                if (i == activeWeapon)
                {
                    Equip(inventory[i], activeWeapon);
                    continue;
                }

                Unequip(inventory[i]);
            }

            //if (Input.GetKeyDown(KeyCode.Alpha1) && inventory[0] != null)
            //{
            //    Equip(inventory[0], 0);
            //    Unequip(inventory[1]);
            //    Unequip(inventory[2]);
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha2) && inventory[1] != null)
            //{
            //    Equip(inventory[1], 1);
            //    Unequip(inventory[0]);
            //    Unequip(inventory[2]);
            //}
            //else if (Input.GetKeyDown(KeyCode.Alpha3) && inventory[2] != null)
            //{
            //    Equip(inventory[2], 2);
            //    Unequip(inventory[1]);
            //    Unequip(inventory[0]);
            //}

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

        void HandleDrop()
        {
            if (inventory[activeWeapon].GetComponent<Weapon>() && inventory[activeWeapon].GetComponent<Weapon>().isEquiped)
            {
                DropItem(activeWeapon);
                // remove sprite
            }
            else if (inventory[activeWeapon].GetComponent<Item>() && inventory[activeWeapon].GetComponent<Item>().isEquiped)
            {
                DropItem(activeWeapon);
                // remove sprite
            }
        }

        void DropItem(int index)
        {
            inventory[index].gameObject.GetComponent<Collider>().enabled = true;
            inventory[index].AddComponent<Rigidbody>();
            inventory[index].transform.SetParent(null);
            inventory[index].layer = 10; // interactable layer
            inventory[index].GetComponent<Rigidbody>().AddForce(Vector3.right * 2, ForceMode.Impulse);

            if (inventory[index].GetComponent<Item>()) inventory[index].GetComponent<Item>().isEquiped = false;
            else if (inventory[index].GetComponent<Weapon>()) inventory[index].GetComponent<Weapon>().isEquiped = false;

            inventory[index] = null;          
            PDAItems[index].GetComponent<Image>().overrideSprite = emptySlot;
        }

        void IsEquipedCheck(int i)
        {
            for (int y = 0; y < inventorySize; y++)
            {
                if (inventory[i] == inventory[y]) return;
                else if (inventory[y].GetComponent<Item>())
                {
                    inventory[y].GetComponent<Item>().isEquiped = false;
                }
                else if (inventory[y].GetComponent<Weapon>())
                {
                    inventory[y].GetComponent<Weapon>().isEquiped = false;
                }
            }
        }

        // ------------------------------------ Equip / Unequip ------------------------------------------------ \\
        void Equip(GameObject item, int slot)
        {
            if (item.GetComponent<Glowstick>()) item.GetComponent<Glowstick>().TurnOn(); // enable light on glowstick, becasue it's special

            if (item.GetComponent<Weapon>()) item.GetComponent<Weapon>().isEquiped = true;
            else item.GetComponent<Item>().isEquiped = true;
            item.gameObject.GetComponent<Renderer>().enabled = true;

            activeWeapon = slot;
            IsEquipedCheck(slot);
        }

        void Unequip(GameObject item)
        {
            if (item.GetComponent<Glowstick>()) item.GetComponent<Glowstick>().TurnOff(); // disable light

            if (item.GetComponent<Weapon>()) item.GetComponent<Weapon>().isEquiped = false;
            else item.GetComponent<Item>().isEquiped = false;
            item.gameObject.GetComponent<Renderer>().enabled = false;
        }

        public void ResetForNewRun()
        {
            for (int i = 0; i < inventory.Length; i++) 
            {
                inventory[i] = null;
            }
        }
    }
}
