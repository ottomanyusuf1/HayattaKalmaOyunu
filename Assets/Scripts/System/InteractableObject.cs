using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
 
public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public string ItemName;
 
    public string GetItemName()
    {
        return ItemName;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            //eğer envanter dolu değilse
            if(InventorySystem.Instance.CheckSlotsAvailable(1))
            {
                InventorySystem.Instance.AddToInventory(ItemName, true);

                InventorySystem.Instance.itemsPickedup.Add(gameObject.name);

                Destroy(gameObject);
            }
            else
            {
                Debug.Log("inventory is full!");

            }
        }
    }

 

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
         if(other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}