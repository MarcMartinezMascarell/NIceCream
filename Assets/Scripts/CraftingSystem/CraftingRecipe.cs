using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public List<CraftingItem> requiredItems;
    public List<CraftingItem> craftedItems;
    public int craftTime = 5;
    public bool isUnlocked = true;
    public bool needsWork = false;
    
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


}

[Serializable]
public struct CraftingItem
{
    public InventoryItemData item;
    public int amount;
}
