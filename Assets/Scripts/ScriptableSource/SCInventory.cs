using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable/Inventory" )]
public class SCInventory : ScriptableObject
{
    public List<Slot> inventorySlots = new List<Slot>();
    int stackLimit = 4;
    public bool AddItem(SCitem item)
    {
        foreach(Slot slot in inventorySlots)
        {
            if(slot.item==item)
            {
                if(slot.item.canStackable)
                {
                    if(slot.itemcount < stackLimit)
                    {
                        slot.itemcount++;
                        if(slot.itemcount >= stackLimit)
                        {
                            slot.isFull = true; 
                        }
                        return true;
                    }
                }
            }
            else if(slot.itemcount==0)
            {
                slot.AddItemToSlot(item);
                return true;
            }
        }
        return false;
        
    }
}

[System.Serializable]
public class Slot
{
    public bool isFull;
    public int itemcount;
    public SCitem item;
    public void AddItemToSlot(SCitem item)
    {
            this.item = item;
            if(item.canStackable == false)
            {
                isFull = true;
            }
            itemcount++;
    }
}