using System.Collections.Generic;
using UnityEngine;

public class InventoryUIControl : MonoBehaviour
{
    public List<SlotUI> uiList = new List<SlotUI>();
    Inventory userInventory;

    private void Start()
    {
        userInventory = gameObject.GetComponent<Inventory>();
    }

    public void UpdateUI()
    {
        for(int i =0;i <uiList.Count; i++)
        {
            if(userInventory.playerInventory.inventorySlots[i].itemcount > 0)
            {
                uiList[i].itemImage.sprite=userInventory.playerInventory.inventorySlots[i].item.itemIcon;
                if (userInventory.playerInventory.inventorySlots[i].item.canStackable == true)
                {
                    uiList[i].itemCountText.gameObject.SetActive(true);
                    uiList[i].itemCountText.text = userInventory.playerInventory.inventorySlots[i].itemcount.ToString();
                }
                else
                {
                    uiList[i].itemCountText.gameObject.SetActive(false);
                }
            }
            else
            {
                uiList[i].itemImage.sprite = null;
                uiList[i].itemCountText.gameObject.SetActive(false);
            }
        }
    }
}
