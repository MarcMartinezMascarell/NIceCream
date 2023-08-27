using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Recipes List")]
public class CraftingRecipesList : ScriptableObject, IEnumerable<RecipeItem>
{
    [SerializeField] private List<CraftingRecipe> _recipes;

    public List<CraftingRecipe> Recipes => _recipes;
    public IEnumerator<RecipeItem> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

[System.Serializable]
public struct RecipeItem
{
    public CraftingRecipe Recipe;
}