using System;
using TMPro;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public TextMeshProUGUI amountTXT;
    public InventoryItem itemInSlot;

    void Update()
    {
        InventoryItem item = CheckInventoryItem();

        if (item != null)
        {
            itemInSlot = item;
        }
        else
        {
            itemInSlot = null;
        }

        if (itemInSlot != null)
        {
            amountTXT.gameObject.SetActive(true);
            amountTXT.text = $"{itemInSlot.amountInInventory}";
            amountTXT.transform.SetAsLastSibling();
        }
        else
        {
            amountTXT.gameObject.SetActive(false);
        }
    }

    private InventoryItem CheckInventoryItem()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<InventoryItem>())
            {
                return child.GetComponent<InventoryItem>();
            }
        }
        return null;
    }

    public void UpdateItemInSlot()
    {
        itemInSlot = CheckInventoryItem();
    }
}
