using System.Collections;
using System.Collections.Generic;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CraftingKeeperDisplay : MonoBehaviour
{
    private CraftingSystem _craftingSystem;
    private PlayerInventoryHolder _playerInventoryHolder;
    private CraftingKeeper _craftingKeeper;
    
    [SerializeField] private GameObject _recipesListPanel;
    [SerializeField] private GameObject _craftingUIItemPrefab;
    [SerializeField] private TextMeshProUGUI _craftingTimerText;
    
    [Header("Item Preview Section")]
    [SerializeField] private Image _itemPreviewImage;
    [SerializeField] private TextMeshProUGUI _itemPreviewName;
    [SerializeField] private TextMeshProUGUI _itemPreviewDescription;
    
    private CraftingRecipe _selectedRecipe;
    
    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public void DisplayCraftingWindow(CraftingSystem craftingSystem, PlayerInventoryHolder playerInventoryHolder, CraftingKeeper craftingKeeper)
    {
        _craftingSystem = craftingSystem;
        _playerInventoryHolder = playerInventoryHolder;
        _craftingKeeper = craftingKeeper;
        
        RefreshDisplay();
    }
    
    private void RefreshDisplay()
    {
        DisplayCraftinItemsList();
    }

    public void CraftSelectedItem()
    {
        if(_selectedRecipe == null) return;
        
        if (!CanCraftSelectedItem())
        {
            Debug.Log("Not enough items to craft");
            return;
        }
        
        foreach (var item in _selectedRecipe.requiredItems)
        {
            _playerInventoryHolder.PrimaryInventorySystem.RemoveFromInventory(item.item, item.amount);
        }
        
        _craftingKeeper.CraftRecipe(_selectedRecipe);
        
        // foreach (var item in _selectedRecipe.craftedItems)
        // {
        //     _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(item.item, item.amount);
        // }
    }

    private void AddToPlayerInventory()
    {
        foreach (var item in _selectedRecipe.craftedItems)
        {
            _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(item.item, item.amount);
        }
    }
    private bool CanCraftSelectedItem()
    {
        if(_selectedRecipe == null) return false;
        
        foreach (var item in _selectedRecipe.requiredItems)
        {
            if (_playerInventoryHolder.PrimaryInventorySystem.GetItemCount(item.item) < item.amount)
            {
                return false;
            }
        }
        
        return true;
    }
    
    private void DisplayCraftinItemsList()
    {
        foreach (Transform child in _recipesListPanel.transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var recipe in _craftingSystem.CraftingRecipesList.Recipes)
        {
            var craftingItemSlotUI = Instantiate(_craftingUIItemPrefab, _recipesListPanel.transform);
            craftingItemSlotUI.GetComponent<CraftingItemSlotUI>().AddRecipe(recipe);
        }
        
        DisplayItemPreview(_craftingSystem.CraftingRecipesList.Recipes[0].craftedItems[0].item);
    }
    
    public void DisplayItemPreview(CraftingRecipe recipe)
    {
        _selectedRecipe = recipe;
        DisplayItemPreview(recipe.craftedItems[0].item);
    }
    
    public void DisplayItemPreview(InventoryItemData itemData)
    {
        _itemPreviewImage.sprite = itemData.Icon;
        _itemPreviewName.text = itemData.DisplayName;
        _itemPreviewDescription.text = itemData.Description;
    }
    public void CleanAllHighlights()
    {
        foreach (Transform child in _recipesListPanel.transform)
        {
            child.GetComponent<CraftingItemSlotUI>().CleanHighlight();
        }
    }

    private IEnumerator WaitRecipeTime()
    {
        yield return new WaitForSeconds(_selectedRecipe.craftTime);
        AddToPlayerInventory();
    }
}
