using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<string> inventory = new List<string>();
    private bool isInventoryOpen = false;

    void Update()
    {
        // "I" tuşuna basınca envanteri aç/kapat
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    // Envanteri aç/kapat
    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        Debug.Log("Envanter " + (isInventoryOpen ? "açildi" : "kapandi"));
        ShowInventory();
    }

    // Eşya ekleme
    public void AddItem(string item)
    {
        inventory.Add(item);
        Debug.Log(item + " envantere eklendi.");
    }

    // Eşya çıkarma
    public void RemoveItem(string item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            Debug.Log(item + " envanterden çikarildi.");
        }
        else
        {
            Debug.Log(item + " envanterde bulunamadi.");
        }
    }

    // Envanteri gösterme
    public void ShowInventory()
    {
        Debug.Log("Envanter: " + string.Join(", ", inventory));
    }

    // Belirli bir eşyanın olup olmadığını kontrol etme
    public bool HasItem(string item)
    {
        return inventory.Contains(item);
    }
}
