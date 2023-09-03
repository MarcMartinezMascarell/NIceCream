using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Inventory_Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    [SerializeField] private ShopKeeperDisplay _shopKeeperDisplay;
    [SerializeField] private CraftingKeeperDisplay _craftingKeeperDisplay;

    private void Awake()
    {
        _shopKeeperDisplay.gameObject.SetActive(false);
        _craftingKeeperDisplay.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            _shopKeeperDisplay.gameObject.SetActive(false);
            _craftingKeeperDisplay.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        ShopKeeper.OnShopWindowRequested += DisplayShopWindow;
        CraftingKeeper.OnCraftingWindowRequested += DisplayCraftingWindow;
        CraftingKeeper.EndAllInteractions += CloseAllWindows;
    }
    
    private void OnDisable()
    {
        ShopKeeper.OnShopWindowRequested -= DisplayShopWindow;
        CraftingKeeper.OnCraftingWindowRequested -= DisplayCraftingWindow;
        CraftingKeeper.EndAllInteractions -= CloseAllWindows;
    }
    
    private void CloseAllWindows()
    {
        _shopKeeperDisplay.gameObject.SetActive(false);
        _craftingKeeperDisplay.gameObject.SetActive(false);
    }
    
    private void DisplayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventoryHolder)
    {
        _shopKeeperDisplay.gameObject.SetActive(true);
        _shopKeeperDisplay.DisplayShopWindow(shopSystem, playerInventoryHolder);
    }
    
    private void DisplayCraftingWindow(CraftingSystem craftingSystem, PlayerInventoryHolder playerInventoryHolder, CraftingKeeper craftingKeeper)
    {
        _craftingKeeperDisplay.gameObject.SetActive(true);
        _craftingKeeperDisplay.DisplayCraftingWindow(craftingSystem, playerInventoryHolder, craftingKeeper);
    }
}
