using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ShopSystem
{
    [SerializeField] private string _shopName;
    [SerializeField] private List<ShopSlot> _shopInventory;
    [SerializeField] private float _money;
    [SerializeField] private float _sellPriceMultiplier;
    [SerializeField] private float _buyPriceMultiplier;
    
    public string ShopName => _shopName;
    public List<ShopSlot> ShopInventory => _shopInventory;
    public float ShopMoney => _money;
    public float BuyPriceMultiplier => _buyPriceMultiplier;
    public float SellPriceMultiplier => _sellPriceMultiplier;
    
    public ShopSystem(string name, int size, int money, float sellPriceMultiplier, float buyPriceMultiplier)
    {
        _shopName = name;
        _money = money;
        _sellPriceMultiplier = sellPriceMultiplier;
        _buyPriceMultiplier = buyPriceMultiplier;
        
        SetShopSize(size);
    }
    
    private void SetShopSize(int size)
    {
        _shopInventory = new List<ShopSlot>(size);
        for (int i = 0; i < size; i++)
        {
            _shopInventory.Add(new ShopSlot());
        }
    }
    
    public void AddToShop(InventoryItemData data, int amount)
    {
        if(ContainsItem(data, out ShopSlot shopSlot))
        {
            shopSlot.AddToStack(amount);
            return;
        }

        var freeSlot = GetFreeSlot();
        freeSlot.AssignItem(data, amount);
    }
    
    private ShopSlot GetFreeSlot()
    {
        var freeSlot = _shopInventory.FirstOrDefault(i => i.ItemData == null);
        if(freeSlot == null)
        {
            freeSlot = new ShopSlot();
            _shopInventory.Add(freeSlot);
        }
        
        return freeSlot;
    }
    
    public bool ContainsItem(InventoryItemData itemToAdd, out ShopSlot shopSlot)
    {
        shopSlot = _shopInventory.Find(i => i.ItemData == itemToAdd);
        return shopSlot != null;
    }

    public void PurchaseItem(InventoryItemData data, int amount)
    {
        if(!ContainsItem(data, out ShopSlot shopSlot)) return;
        
        shopSlot.RemoveFromStack(amount);
    }

    public void GainGold(float basketTotal)
    {
        _money += basketTotal;
    }

    public void SellItem(InventoryItemData itemKey, int itemValue, float price)
    {
        AddToShop(itemKey, itemValue);
        ReduceGold(price);
    }

    private void ReduceGold(float price)
    {
        _money -= price;
    }
}
