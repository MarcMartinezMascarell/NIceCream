using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop System/Shop Item List")]
public class ShopItemList : ScriptableObject
{
    [SerializeField] private List<ShopInventoryItem> _items;
    [SerializeField] private int _maxAllowedMoney;
    [SerializeField] private float _sellPriceMultiplier = 1.5f;
    [SerializeField] private float _buyPriceMultiplier = 0.5f;
    
    public List<ShopInventoryItem> Items => _items;
    public int MaxAllowedMoney => _maxAllowedMoney;
    public float SellPriceMultiplier => _sellPriceMultiplier;
    public float BuyPriceMultiplier => _buyPriceMultiplier;
}

[System.Serializable]
public struct ShopInventoryItem
{
    public InventoryItemData ItemData;
    public int Amount;
    public float Price;
}