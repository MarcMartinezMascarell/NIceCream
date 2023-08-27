using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingItemSlotUI : MonoBehaviour
{
    [SerializeField] private GameObject _craftingItemUIPrefab;
    [SerializeField] private GameObject _requiredItemsPanel;
    [SerializeField] private GameObject _craftedItemsPanel;
    
    private InventoryItemData _itemData;
    private Button _previewButton;
    private CraftingRecipe _recipe;
    public CraftingKeeperDisplay ParentDisplay { get; set; }

    private void Awake()
    {
        foreach (Transform child in _requiredItemsPanel.transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (Transform child in _craftedItemsPanel.transform)
        {
            Destroy(child.gameObject);
        }

        _previewButton = GetComponent<Button>();
        
        _previewButton.onClick.AddListener(PreviewItem);
        ParentDisplay = transform.parent.GetComponentInParent<CraftingKeeperDisplay>();
    }
    
    public void PreviewItem()
    {
        ParentDisplay.DisplayItemPreview(_recipe);
    }

    public void AddRecipe(CraftingRecipe recipe)
    {
        _recipe = recipe;
        foreach (var item in recipe.requiredItems)
        {
            var craftingItemUIPrefab = Instantiate(_craftingItemUIPrefab, _requiredItemsPanel.transform);
            var craftingItemUI = craftingItemUIPrefab.GetComponent<CraftingItemUI>();
            craftingItemUI.SetItem(item.item, item.amount, craftingItemUI.AssignedCraftingSlot);
        }
        
        foreach (var item in recipe.craftedItems)
        {
            var craftingItemUIPrefab = Instantiate(_craftingItemUIPrefab, _craftedItemsPanel.transform);
            var craftingItemUI = craftingItemUIPrefab.GetComponent<CraftingItemUI>();
            craftingItemUI.SetItem(item.item, item.amount, craftingItemUI.AssignedCraftingSlot);
            _itemData = item.item;
        }
    }
}
