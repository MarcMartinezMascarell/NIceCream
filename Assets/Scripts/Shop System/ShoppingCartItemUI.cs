using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingCartItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private Image _itemSprite;
    
    public void SetItem(InventoryItemData itemData, int amount)
    {
        _amountText.text = amount.ToString();
        _itemSprite.sprite = itemData.Icon;
    }
}
