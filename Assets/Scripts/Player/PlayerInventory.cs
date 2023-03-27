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
        public bool inventoryFull;
        public string fullInventoryTooltip;
        [SerializeField] Sprite emptySlot;

        Vector3 itemPos = new Vector3(.35f, -.35f, 1f);
        Quaternion itemRot = Quaternion.Euler(0f, -90f, 0f);

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
            if (GameManager.gameManager.gameState != GameManager.gameStates.gameplay) return;
            HandleEquipUnequip(); // Checks for the input to switch to weapons
            HandleItemUsage(); // checks to see if an item is used, deletes it if so
            InventorySlotUpdate();
            AdjustEquippedItemLocalPos();
            //Debug.Log(inventory[activeWeapon].transform.position + " " + inventory[activeWeapon].gameObject.name);
            if (Input.GetKeyDown(KeyCode.Q)) HandleDrop(); // checks the input for droping an item
        }

        // ---------------------------------- Inventory --------------------------------------- \\
        public void AddToInventory(GameObject itemToAdd)
        {
            int firstOpenSlot = 99;
            for (int i = 0; i < inventory.Length; i++) // finds first available inventory slot for item
            {
                if (inventory[i] == null)
                {
                    firstOpenSlot = i;
                    //Debug.Log(firstOpenSlot);
                    break;
                }
            }

            if (firstOpenSlot > inventory.Length - 1)inventoryFull = true;
            else inventoryFull = false;
            

            if (firstOpenSlot > inventory.Length - 1) return;

            inventory[firstOpenSlot] = itemToAdd;
            inventory[firstOpenSlot].gameObject.GetComponent<Collider>().enabled = false;

            inventory[firstOpenSlot].transform.SetParent(FirstPersonController_Sam.fpsSam.playerCamera.transform);
            inventory[firstOpenSlot].transform.position = Vector3.zero;
            Debug.Log(inventory[firstOpenSlot].transform.position + " " + inventory[firstOpenSlot].gameObject.name + " " + firstOpenSlot);
            inventory[firstOpenSlot].transform.localPosition = itemPos;
            inventory[firstOpenSlot].layer = 0;
            

            if (!inventory[firstOpenSlot].GetComponent<HarpoonGun>()) itemToAdd.transform.localRotation = itemRot;
            else inventory[firstOpenSlot].transform.localRotation = Quaternion.Euler(0, 0, 0);

            if (inventory[firstOpenSlot].GetComponent<Rigidbody>()) Destroy(itemToAdd.GetComponent<Rigidbody>());
            if (inventory[firstOpenSlot].GetComponent<Weapon>()) inventory[firstOpenSlot].GetComponent<Weapon>().playerCamera = playerCam;
            

            for (int j = 0; j < inventory.Length; j++) // DONT REMOVE
            {
                if (j == firstOpenSlot) Equip(j);
                else if (inventory[j] != null) Unequip(inventory[j]);
            }
        }

        // ------------------------------- Update Loop ------------------------------------------ \\
        void HandleEquipUnequip() // handles the old equip / unequip system
        {


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
                    if (inventory[i].GetComponent<Item>() != null)
                    {
                        if (inventory[i].GetComponent<Item>().isUsed) continue;
                    }
                    Equip(activeWeapon);
                    continue;
                }

                Unequip(inventory[i]);
            }
        }

        void AdjustEquippedItemLocalPos()
        {
            if (inventory[activeWeapon] == null) return;
            if (inventory[activeWeapon].transform.localPosition != itemPos) inventory[activeWeapon].transform.localPosition = itemPos;
            if (inventory[activeWeapon].transform.localRotation != itemRot) inventory[activeWeapon].transform.localRotation = itemRot;
        }

        void InventorySlotUpdate()
        {
            if (UI_Manager.ui_Manager.PDAOpen() || InputManager.inputManager.lastNumKeyPressed == activeWeapon) return;
            activeWeapon = InputManager.inputManager.lastNumKeyPressed;
            for (int i = 0; i < UI_Manager.ui_Manager.inventoryButtons.Length; i++)
            {
                if (InventoryButton.caller == null) InventoryButton.caller = UI_Manager.ui_Manager.inventoryButtons[0];
                if (UI_Manager.ui_Manager.inventoryButtons[i].slot == InventoryButton.caller.slot)
                {
                    //Debug.LogError("Setting Caller to active weapon slot, caller is now slot: " + (activeWeapon));
                    UI_Manager.ui_Manager.inventoryButtons[activeWeapon].SetThisAsCaller();
                }
            }
            
        }

        void HandleItemUsage()
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i] != null && inventory[i].GetComponent<Item>() && inventory[i].GetComponent<Item>().isUsed)
                {
                    inventory[i].GetComponent<Item>().isEquiped = false;
                    inventory[i] = null;
                    inventoryFull = false;
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
            inventoryFull = false;
            inventory[index].gameObject.GetComponent<Collider>().enabled = true;
            inventory[index].AddComponent<Rigidbody>();
            inventory[index].transform.SetParent(null);
            inventory[index].layer = 10; // interactable layer
            inventory[index].GetComponent<Rigidbody>().AddForce(Vector3.right * 2, ForceMode.Impulse);

            if (inventory[index].GetComponent<Item>()) inventory[index].GetComponent<Item>().isEquiped = false;
            else if (inventory[index].GetComponent<Weapon>()) inventory[index].GetComponent<Weapon>().isEquiped = false;

            inventory[index] = null;          
           // PDAItems[index].GetComponent<Image>().overrideSprite = emptySlot;
        }

        void IsEquipedCheck(int i)
        {
            for (int y = 0; y < inventorySize; y++)
            {
                if (inventory[y] == null || inventory[i] == null) return;
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
        void Equip(int slot)
        {
            if (inventory[slot] == null) return;
            if (inventory[slot].GetComponent<Glowstick>()) inventory[slot].GetComponent<Glowstick>().TurnOn(); // enable light on glowstick, becasue it's special

            if (inventory[slot].GetComponent<Weapon>()) inventory[slot].GetComponent<Weapon>().isEquiped = true;
            else inventory[slot].GetComponent<Item>().isEquiped = true;
            inventory[slot].gameObject.SetActive(true);

            //activeWeapon = slot;
            IsEquipedCheck(slot);
        }

        void Unequip(GameObject item)
        {
            if (item.GetComponent<Glowstick>()) item.GetComponent<Glowstick>().TurnOff(); // disable light

            if (item.GetComponent<Weapon>()) item.GetComponent<Weapon>().isEquiped = false;
            else item.GetComponent<Item>().isEquiped = false;
            item.gameObject.SetActive(false);
        }

        public void ResetForNewRun()
        {
            for (int i = 0; i < inventory.Length; i++) 
            {
                inventory[i] = null;
            }
            inventoryFull = false;
            activeWeapon = 0;
        }
    }
}
