using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Unity.VisualScripting;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;
    [SerializeField] private float _money;

    public float Money => _money;
    //private int inventorySize;

    public List<InventorySlot> InventorySlots => inventorySlots;
    
    public int InventorySize => InventorySlots.Count;
    
    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public InventorySystem(int size)
    {
        _money = 0;
        CreateInventory(size);
    }
    public InventorySystem(int size, float money)
    {
        _money = money;
        CreateInventory(size);
    }

    private void CreateInventory(int size)
    {
        inventorySlots = new List<InventorySlot>(size);
        
        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot, int offset = 0)
    {
        invSlot = InventorySlots.Skip(offset).Where(i => i.ItemData == itemToAdd).ToList();
        // if (invSlot.Count == 0)
        // {
        //     invSlot = InventorySlots.Where(i => i.ItemData == itemToAdd).ToList();
        // }
        

        return invSlot.Count > 0;
    }

    public bool HasFreeSlot(out InventorySlot invSlot, int offset = 0)
    {
        invSlot = InventorySlots.Skip(offset).FirstOrDefault(i => i.ItemData == null);
        if(invSlot == null)
            invSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null);
        return invSlot == null ? false : true;
    }

    public bool CheckInventoryRemaining(Dictionary<InventoryItemData, int> itemsToCheck)
    {
        var clonedSystem = new InventorySystem(this.InventorySize);

        for (int i = 0; i < InventorySize; i++)
        {
            clonedSystem.InventorySlots[i].AssignItem(this.InventorySlots[i].ItemData, this.InventorySlots[i].StackSize);
        }
        
        foreach (var item in itemsToCheck)
        {
            for (int i = 0; i < item.Value; i++)
            {
                if(!clonedSystem.AddToInventory(item.Key, 1)) return false;
            }
        }
        
        return true;
    }
    
    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd, int offset = 0)
    {
        if (ContainsItem(itemToAdd, out List<InventorySlot> invSlot, offset)) //Check if item exists in inventory
        {
            foreach (var slot in invSlot)
            {
                if (slot.RoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    
                    return true;
                }
            }

        }
        
        if(HasFreeSlot(out InventorySlot freeSlot, offset)) //Gets the first available slot
        {
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }

        return false;
    }

    public void SpendGold(float basketTotal)
    {
        _money -= basketTotal;
    }

    public Dictionary<InventoryItemData, int> GetAllItemsHeld()
    {
        var distinctItems = new Dictionary<InventoryItemData, int>();
        
        foreach (var slot in InventorySlots)
        {
            if (slot.ItemData == null) continue;

            if (distinctItems.ContainsKey(slot.ItemData))
                distinctItems[slot.ItemData] += slot.StackSize;
            else
                distinctItems.Add(slot.ItemData, slot.StackSize);
        }
        
        return distinctItems;
    }

    public void GainGold(float price)
    {
        _money += price;
    }

    public void RemoveFromInventory(InventoryItemData data, int amount)
    {
        if (ContainsItem(data, out List<InventorySlot> invSlot))
        {
            foreach (var slot in invSlot)
            {
                var stackSize = slot.StackSize;
                
                if (stackSize > amount) slot.RemoveFromStack(amount);
                else
                {
                    slot.RemoveFromStack(stackSize);
                    amount -= stackSize;
                }
                
                OnInventorySlotChanged?.Invoke(slot);
                
            }
        }
    }
}
