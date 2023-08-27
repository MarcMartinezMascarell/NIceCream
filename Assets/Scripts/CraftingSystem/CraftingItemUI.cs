using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingItemUI : MonoBehaviour
{
    [SerializeField] private Image _itemSprite;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private CraftingSlot _assignedCraftingSlot;
    
    public CraftingSlot AssignedCraftingSlot => _assignedCraftingSlot;

    private void Awake()
    {
        _itemSprite.sprite = null;
        _itemSprite.color = Color.clear;
        _amountText.text = string.Empty;
    }

    public void SetItem(InventoryItemData itemData, int amount, CraftingSlot craftingSlot)
    {
        _itemSprite.sprite = itemData.Icon;
        _itemSprite.color = Color.white;
        _amountText.text = amount.ToString();
        _assignedCraftingSlot = craftingSlot;
    }
    
    
    
}
