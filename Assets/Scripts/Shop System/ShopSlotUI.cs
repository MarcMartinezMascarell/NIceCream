using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    [SerializeField] private Image _itemSprite;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemAmount;
    [SerializeField] private TextMeshProUGUI _itemPrice;
    [SerializeField] private ShopSlot _assignedItemSlot;
    
    [SerializeField] private Button _addItemToCartButton;
    [SerializeField] private Button _removeItemFromCartButton;
    
    private int _tempAmount;
    
    public ShopKeeperDisplay ParentDisplay { get; private set; }
    public ShopSlot AssignedItemSlot => _assignedItemSlot;
    public float PriceMultiplier { get; private set; }

    private void Awake()
    {
        _itemSprite.sprite = null;
        _itemSprite.color = Color.clear;
        _itemName.text = string.Empty;
        _itemAmount.text = string.Empty;
        _itemPrice.text = string.Empty;
        
        _addItemToCartButton?.onClick.AddListener(AddItemToCart);
        _removeItemFromCartButton?.onClick.AddListener(RemoveItemFromCart);
        ParentDisplay = transform.parent.GetComponentInParent<ShopKeeperDisplay>();
    }
    
    public void Init(ShopSlot slot, float priceMultiplier)
    {
        _assignedItemSlot = slot;
        UpdateUISlot(slot, priceMultiplier);
        _tempAmount = slot.StackSize;
        PriceMultiplier = priceMultiplier;
    }

    private void UpdateUISlot(ShopSlot shopSlot, float priceMultiplier)
    {
        if (_assignedItemSlot.ItemData != null)
        {
            _itemSprite.sprite = _assignedItemSlot.ItemData.Icon;
            _itemSprite.color = Color.white;
            _itemName.text = $"{_assignedItemSlot.ItemData.DisplayName}";
            _itemAmount.text = _assignedItemSlot.StackSize.ToString();
            var modifiedPrice = ShopKeeperDisplay.GetModifiedPrice(_assignedItemSlot.ItemData, 1, priceMultiplier);
            _itemPrice.text = $"{modifiedPrice}";
        }
        else
        {
            _itemSprite.sprite = null;
            _itemSprite.color = Color.clear;
            _itemName.text = string.Empty;
            _itemAmount.text = string.Empty;
            _itemPrice.text = string.Empty;
        }
        
    }
    
    private void AddItemToCart()
    {
        if (_tempAmount <= 0) return;
        
        _tempAmount--;
        _itemAmount.text = _tempAmount.ToString();
        ParentDisplay.AddItemToCart(this);
        
    }
    
    private void RemoveItemFromCart()
    {
        if (_tempAmount == _assignedItemSlot.StackSize) return;
        
        _tempAmount++;
        _itemAmount.text = _tempAmount.ToString();
        ParentDisplay.RemoveItemFromCart(this);
    }
    
}
