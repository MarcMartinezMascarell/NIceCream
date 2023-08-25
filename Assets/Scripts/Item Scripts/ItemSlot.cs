using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSlot : ISerializationCallbackReceiver
{
    [NonSerialized] protected InventoryItemData itemData;
    [SerializeField] protected int itemID = -1;
    [SerializeField] protected int stackSize;

    public InventoryItemData ItemData => itemData;
    public int StackSize => stackSize;
    
    public void ClearSlot()
    {
        itemData = null;
        itemID = -1;
        stackSize = -1;
    }
    
    public void AssignItem(InventorySlot invSlot)
    {
        if(itemData == invSlot.ItemData) AddToStack(invSlot.stackSize);
        else
        {
            itemData = invSlot.ItemData;
            itemID = itemData.ID;
            stackSize = 0;
            AddToStack(invSlot.stackSize);
        }
    }
    
    public void AssignItem(InventoryItemData data, int amount)
    {
        if(itemData == data) AddToStack(amount);
        else
        {
            itemData = data;
            itemID = itemData.ID;
            stackSize = 0;
            AddToStack(amount);
        }
    }
    
    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        if (itemID == -1) return;

        var db = Resources.Load<Database>("Database");
        itemData = db.GetItem(itemID);
    }

    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
        if(stackSize <= 0) ClearSlot();
    }
}
