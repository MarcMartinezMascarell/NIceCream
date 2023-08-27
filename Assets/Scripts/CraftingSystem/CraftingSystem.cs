using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftingSystem
{
    [SerializeField] private string _craftingName;
    // [SerializeField] private List<CraftingSlot> _craftingInventory;
    [SerializeField] private CraftingRecipesList _craftingRecipes;

    public string CraftingName => _craftingName;
    public CraftingRecipesList CraftingRecipesList => _craftingRecipes;
    
    public CraftingSystem(string name)
    {
        _craftingName = name;
    }

    public void AddCraftingRecipes(CraftingRecipesList recipes)
    {
        _craftingRecipes = recipes;
    }
}

[System.Serializable]
public class CraftingSlot : ItemSlot
{
    public CraftingSlot()
    {
        ClearSlot();
    }
}