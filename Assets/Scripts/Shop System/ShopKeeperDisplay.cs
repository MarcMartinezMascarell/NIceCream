using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Inventory;
using Inventory.Inventory_Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopKeeperDisplay : MonoBehaviour
{
    [SerializeField] private ShopSlotUI _shopSlotPrefab;
    [SerializeField] private ShoppingCartItemUI _shoppingCartItemPrefab;
    [SerializeField] private Button _buyTab;
    [SerializeField] private Button _sellTab;
    [SerializeField] private TextMeshProUGUI _shopNameText;
    
    [Header("Shopping Cart")]
    [SerializeField] private TextMeshProUGUI _basketTotalText;
    [SerializeField] private TextMeshProUGUI _playerMoneyText;
    [SerializeField] private TextMeshProUGUI _shopMoneyText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _buyButtonText;
    
    [Header("Item Preview Section")]
    [SerializeField] private Image _itemPreviewImage;
    [SerializeField] private TextMeshProUGUI _itemPreviewName;
    [SerializeField] private TextMeshProUGUI _itemPreviewDescription;
    
    [SerializeField] private GameObject _itemListContentPanel;
    [SerializeField] private GameObject _shoppingCartContentPanel;
    
    private ShopSystem _shopSystem;
    private PlayerInventoryHolder _playerInventoryHolder;
    private float _basketTotal;
    private bool _isSelling;
    
    private Dictionary<InventoryItemData, int> _shoppingCart = new Dictionary<InventoryItemData, int>();
    private Dictionary<InventoryItemData, ShoppingCartItemUI> _shoppingCartUI = new Dictionary<InventoryItemData, ShoppingCartItemUI>();

    public void DisplayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventoryHolder)
    {
        _shopSystem = shopSystem;
        _playerInventoryHolder = playerInventoryHolder;
        
        RefreshDisplay();
    }
    
    private void RefreshDisplay()
    {
        if (_buyButton != null)
        {
            _buyButtonText.text = _isSelling ? "Sell Items" : "Buy Items";
            _buyButton.onClick.RemoveAllListeners();
            if(_isSelling)
                _buyButton.onClick.AddListener(SellItems);
            else
                _buyButton.onClick.AddListener(BuyItems);
        }
        
        ClearSlot();
        ClearItemPreview();
        
        _shopNameText.text = _shopSystem.ShopName;
        _basketTotalText.enabled = false;
        _buyButton.gameObject.SetActive(false);
        _basketTotal = 0;
        _playerMoneyText.text = $"{_playerInventoryHolder.PrimaryInventorySystem.Money}";
        _shopMoneyText.text = $"Shop Money: {_shopSystem.ShopMoney}";
        
        if(!_isSelling) DisplayShopInventory();
        else DisplayPlayerInventory();
    }

    private void SellItems()
    {
        if(_shopSystem.ShopMoney < _basketTotal)
        {
            _basketTotalText.text = "Not enough money!";
            _basketTotalText.color = Color.red;
            return;
        }
        
        foreach (var item in _shoppingCart)
        {
            var price = GetModifiedPrice(item.Key, item.Value, _shopSystem.SellPriceMultiplier);
            _shopSystem.SellItem(item.Key, item.Value, price);
            
            _playerInventoryHolder.PrimaryInventorySystem.GainGold(price);
            _playerInventoryHolder.PrimaryInventorySystem.RemoveFromInventory(item.Key, item.Value);
        }
        
        RefreshDisplay();
    }

    private void BuyItems()
    {
        if (_basketTotal > _playerInventoryHolder.PrimaryInventorySystem.Money)
        {
            _basketTotalText.text = "Not enough money!";
            _basketTotalText.color = Color.red;
            return;
        }
        
        foreach (var item in _shoppingCart)
        {
            _shopSystem.PurchaseItem(item.Key, item.Value);

            for (int i = 0; i < item.Value; i++)
            {
                _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(item.Key, 1);
            }
        }
        
        _shopSystem.GainGold(_basketTotal);
        _playerInventoryHolder.PrimaryInventorySystem.SpendGold(_basketTotal);
        
        RefreshDisplay();
    }
    
    private void ClearSlot()
    {
        _shoppingCart.Clear();
        _shoppingCartUI.Clear();

        foreach (var item in _itemListContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
        
        foreach (var item in _shoppingCartContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
    }

    private void DisplayShopInventory()
    {
        foreach(var item in _shopSystem.ShopInventory)
        {
            if(item.ItemData == null) continue;
            
            var shopSlot = Instantiate(_shopSlotPrefab, _itemListContentPanel.transform);
            shopSlot.Init(item, _shopSystem.BuyPriceMultiplier);
        }
    }

    public void AddItemToCart(ShopSlotUI shopSlotUI)
    {
        var data = shopSlotUI.AssignedItemSlot.ItemData;

        UpdateItemPreview(shopSlotUI);
        
        float price = GetModifiedPrice(data, 1, shopSlotUI.PriceMultiplier);
        
        if(_shoppingCart.ContainsKey(data))
        {
            _shoppingCart[data]++;
            _shoppingCartUI[data].SetItem(data, _shoppingCart[data]);
        }
        else
        {
            _shoppingCart.Add(data, 1);
            var shoppingCartItem = Instantiate(_shoppingCartItemPrefab, _shoppingCartContentPanel.transform);
            shoppingCartItem.SetItem(data, 1);
            _shoppingCartUI.Add(data, shoppingCartItem);
        }
        
        _basketTotal += price;
        //Round to 2 decimals
        _basketTotal = Mathf.Round(_basketTotal * 100f) / 100f;
        _basketTotalText.enabled = true;
        _basketTotalText.text = $"Total: {_basketTotal}";

        if (_basketTotal > 0 && _basketTotalText.IsActive())
        {
            _basketTotalText.enabled = true;
            _buyButton.gameObject.SetActive(true);
        }
        
        CheckCartVsPlayerMoney();
    }
    
    public void RemoveItemFromCart(ShopSlotUI shopSlotUI)
    {
        var data = shopSlotUI.AssignedItemSlot.ItemData;
        
        if(_shoppingCart.ContainsKey(data))
        {
            _shoppingCart[data]--;
            _shoppingCartUI[data].SetItem(data, _shoppingCart[data]);
            
            if(_shoppingCart[data] <= 0)
            {
                _shoppingCart.Remove(data);
                Destroy(_shoppingCartUI[data].gameObject);
                _shoppingCartUI.Remove(data);
            }
        }
        
        _basketTotal -= GetModifiedPrice(data, 1, shopSlotUI.PriceMultiplier);
        //Round to 2 decimals
        _basketTotal = Mathf.Round(_basketTotal * 100f) / 100f;
        _basketTotalText.text = $"Total: {_basketTotal}";
        
        if (_basketTotal <= 0 && _basketTotalText.IsActive())
        {
            _basketTotalText.enabled = false;
            _buyButton.gameObject.SetActive(false);
            ClearItemPreview();
        }
        
        CheckCartVsPlayerMoney();
    }

    private void ClearItemPreview()
    {
        _itemPreviewImage.sprite = null;
        _itemPreviewImage.color = Color.clear;
        _itemPreviewName.text = string.Empty;
        _itemPreviewDescription.text = string.Empty;
    }

    private void CheckCartVsPlayerMoney()
    {
        var moneyToCheck = _isSelling ? _shopSystem.ShopMoney : _playerInventoryHolder.PrimaryInventorySystem.Money;
        _basketTotalText.color = _basketTotal > moneyToCheck ? Color.red : Color.white;
        
        if(_isSelling || _playerInventoryHolder.PrimaryInventorySystem.CheckInventoryRemaining(_shoppingCart)) return;
        
        _basketTotalText.text = "Not enough space in inventory!";
        _basketTotalText.color = Color.red;
        _buyButton.gameObject.SetActive(false);
    }
    
    public static float GetModifiedPrice(InventoryItemData data, int amount, float priceMultiplier)
    {
        var defaultPrice = data.DefaultPrice;
        var modifiedPrice = defaultPrice * priceMultiplier * amount;
        
        //Return float rounded to 2 decimals
        return Mathf.Round(modifiedPrice * 100f) / 100f;
    }
    
    private void UpdateItemPreview(ShopSlotUI shopSlotUI)
    {
        _itemPreviewImage.sprite = shopSlotUI.AssignedItemSlot.ItemData.Icon;
        _itemPreviewName.text = shopSlotUI.AssignedItemSlot.ItemData.DisplayName;
        _itemPreviewDescription.text = shopSlotUI.AssignedItemSlot.ItemData.Description;
    }
    
    private void DisplayPlayerInventory()
    {
        foreach (var item in _playerInventoryHolder.PrimaryInventorySystem.GetAllItemsHeld())
        {
            var tempSlot = new ShopSlot();
            tempSlot.AssignItem(item.Key, item.Value);
            
            var shopSlot = Instantiate(_shopSlotPrefab, _itemListContentPanel.transform);
            shopSlot.Init(tempSlot, _shopSystem.SellPriceMultiplier);
        }
    }
    
    public void OnBuyTabPressed()
    {
        _isSelling = false;
        RefreshDisplay();
    }
    
    public void OnSellTabPressed()
    {
        _isSelling = true;
        RefreshDisplay();
    }
}
