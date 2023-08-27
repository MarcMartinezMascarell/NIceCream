using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public List<CraftingItem> requiredItems;
    public List<CraftingItem> craftedItems;
    
    public bool CanCraft(InventorySystem inventorySystem)
    {
        foreach (var item in requiredItems)
        {
            if (inventorySystem.GetItemCount(item.item) < item.amount)
            {
                return false;
            }
        }

        return true;
    }
    
    public void Craft(InventorySystem inventorySystem)
    {
        if (!CanCraft(inventorySystem)) return;
        
        foreach (var item in requiredItems)
        {
            inventorySystem.RemoveFromInventory(item.item, item.amount);
        }
        
        foreach (var item in craftedItems)
        {
            inventorySystem.AddToInventory(item.item, item.amount);
        }
    }
    
    
}

[Serializable]
public struct CraftingItem
{
    public InventoryItemData item;
    public int amount;
}
