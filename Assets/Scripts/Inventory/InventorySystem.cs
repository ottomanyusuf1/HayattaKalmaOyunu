using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public GameObject ItemInfoUI;
   public static InventorySystem Instance { get; set; }
 
    public GameObject inventoryScreenUI;

    public List<InventorySlot> slotList = new List<InventorySlot>();
    public List<string> itemList = new List<string>();
    private GameObject itemToAdd;
    private InventorySlot whatSlotToEquip;
    public bool isOpen;
    //public bool isFull;

    public TextMeshProUGUI currencyUI;

    //pickup popup
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;

    public List<string> itemsPickedup;

    public int stackLimit = 1;

    internal int currentCoins = 100;
 
 
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
 
 
    void Start()
    {
        isOpen = false;
        

        PopulateSlotList();
        Cursor.visible = false;

    }

    private void PopulateSlotList()
    {

        foreach(Transform child in inventoryScreenUI.transform)
        {
            if(child.CompareTag("Slot"))
            {
                InventorySlot slot = child.GetComponent<InventorySlot>();
                slotList.Add(slot);
            }
        }
    }
 
 
    void Update()
    {
 
        if (Input.GetKeyDown(KeyCode.E) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {
            MovmentManager.Instance.EnableLook(false);

            OpenUI(); 
        }
        else if (Input.GetKeyDown(KeyCode.E) && isOpen)
        {
            MovmentManager.Instance.EnableLook(true);

            CloseUI();
        }

        currencyUI.text = $"{currentCoins} Coins";
    }

    public void OpenUI()
    {
        inventoryScreenUI.SetActive(true);
            inventoryScreenUI.GetComponentInParent<Canvas>().sortingOrder = MenuManager.Instance.SetAsFront();


            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
            isOpen = true;

            ReCalculateList();
    }

    public void CloseUI()
    {
        inventoryScreenUI.SetActive(false);
            if(!CraftingSystem.Instance.isOpen && !StorageManager.Instance.storageUIOpen 
            && !CampfireUIManager.Instance.isUiOpen
            && !BuySystem.Instance.ShopKeeper.isTalkingWithPlayer)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }
            isOpen = false;
    }
 

    public void AddToInventory(string ItemName, bool shouldStack)
    {
       InventorySlot stack = CheckIfStackExists(ItemName);

       if (stack != null && shouldStack)
       {
            stack.GetComponent<InventorySlot>().itemInSlot.amountInInventory += 1;
            stack.UpdateItemInSlot();
       }
       else
       {
            whatSlotToEquip = FindNextEmptySlot();

            itemToAdd = Instantiate(Resources.Load<GameObject>(ItemName),whatSlotToEquip.transform.position,whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);

            itemList.Add(ItemName);
       }

        SoundManager.Instance.PlaySound(SoundManager.Instance.pickupItemSound);

        TriggerPickupPopOp(ItemName, itemToAdd.GetComponent<Image>().sprite);

        ReCalculateList();
        CraftingSystem.Instance.RefleshNeededItems();

        QuestManager.Instance.RefleshQuestList();
    }

    private InventorySlot CheckIfStackExists(string itemName)
    {
        foreach (InventorySlot inventorySlot in slotList)
        {

            inventorySlot.UpdateItemInSlot();
            if (inventorySlot != null && inventorySlot.itemInSlot != null)
            {
                if (inventorySlot.itemInSlot.thisName == itemName
                    && inventorySlot.itemInSlot.amountInInventory < stackLimit)
                    {
                        return inventorySlot;
                    }

            }
        }
        return null;
    }

    void TriggerPickupPopOp(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);
        
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;

        StartCoroutine(HidePickupPopup());
    }

    IEnumerator HidePickupPopup()
{
    yield return new WaitForSeconds(1f); // 1 saniye bekle
    pickupAlert.SetActive(false); // Popup'u gizle
}


    private InventorySlot FindNextEmptySlot()
    {
        foreach(InventorySlot slot in slotList)
        {
            if(slot.transform.childCount <= 1)
            {
                return slot;
            }
        }
        return new InventorySlot();
    }
    public bool CheckSlotsAvailable(int emptyMeeded)
    {
        int emptySlot = 0;

        foreach (InventorySlot slot in slotList)
        {
            if(slot.transform.childCount <= 1)
            {
                emptySlot += 1;
            }
        }

    
        if(emptySlot >= emptyMeeded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveItem(string itemName, int amountToRemove)
    {
        int remainingAmountToRemove = amountToRemove;

        // Iterate over the amount to remove
        while (remainingAmountToRemove != 0)
        {
            int previousRemainingAmount = remainingAmountToRemove;

            //Interate over inventory slots
            foreach (InventorySlot slot in slotList)
            {
                // If the slot is not empty and contains the item we want ro remove
                if (slot.itemInSlot != null && slot.itemInSlot.thisName == itemName)
                {
                    // Decrease the amount of the item in the slot
                    slot.itemInSlot.amountInInventory--;
                    remainingAmountToRemove--;

                    // Remove item from slot if its amount become zero
                    if (slot.itemInSlot.amountInInventory ==0)
                    {
                        Destroy(slot.itemInSlot.gameObject);
                        slot.itemInSlot = null;
                    }
                    break; // Exit the foreach loop if the item was found and removed
                }
            }

            // This should never happen, but we should check this to pevent an endless loop ehile indebug
            if (previousRemainingAmount == remainingAmountToRemove)
            {
                Debug.Log("Item not found of insufficient quantity in inventory");
                break;
            }
            ReCalculateList();
            CraftingSystem.Instance.RefleshNeededItems();
            QuestManager.Instance.RefleshQuestList();
        }
    }

    public void ReCalculateList()
    {
       itemList.Clear();

       foreach (InventorySlot inventorySlot in slotList)
       {
           
        InventoryItem item = inventorySlot.itemInSlot;

        if (item != null)
        {
            if (item.amountInInventory > 0)
             {
                for (int i = 0; i < item.amountInInventory; i++)
                {
                        itemList.Add(item.thisName);
                  }
             }
          }
        
       }
    }


    public int CheckItemAmount(string name)
    {
        int itemCounter = 0;
        foreach (string item in itemList)
        {
            if (item == name)
            {
                itemCounter++;
            }
        }
        return itemCounter;
    }
}