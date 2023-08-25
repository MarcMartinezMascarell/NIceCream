using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Inventory.Inventory_Scripts;
using SaveLoadSystem;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueId))]
public class ShopKeeper : MonoBehaviour, IInteractable
{
    [SerializeField] private ShopItemList _shopItemsHeld;
    [SerializeField] private ShopSystem _shopSystem;
    
    public static UnityAction<ShopSystem, PlayerInventoryHolder> OnShopWindowRequested { get; set; }

    private string _id;
    private ShopSaveData _shopSaveData;

    private void Awake()
    {
        _shopSystem = 
            new ShopSystem(_shopSystem.ShopName, _shopItemsHeld.Items.Count, _shopItemsHeld.MaxAllowedMoney, _shopItemsHeld.SellPriceMultiplier, _shopItemsHeld.BuyPriceMultiplier);
        
        foreach (var item in _shopItemsHeld.Items)
        {
            _shopSystem.AddToShop(item.ItemData, item.Amount);
        }
        
        _id = GetComponent<UniqueId>().ID;
        _shopSaveData = new ShopSaveData(_shopSystem);
    }

    private void Start()
    {
        if(SaveGameManager.data._shopKeeperDictonary.ContainsKey(_id))
        {
            _shopSaveData = SaveGameManager.data._shopKeeperDictonary[_id];
            _shopSystem = _shopSaveData.ShopSystem;
        }
        else
            SaveGameManager.data._shopKeeperDictonary.Add(_id, _shopSaveData);
    }

    private void OnEnable()
    {
        SaveLoad.OnLoadGame += LoadShop;
    }

    private void LoadShop(SaveData data)
    {
        if (!data._shopKeeperDictonary.TryGetValue(_id, out ShopSaveData shopSaveData)) return;

        _shopSaveData = shopSaveData;
        _shopSystem = _shopSaveData.ShopSystem;
    }

    private void OnDisable()
    {
        SaveLoad.OnLoadGame -= LoadShop;
    }

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    public void Interact(Interactor interactor, out bool interactSuccesful)
    {
        var playerInv = interactor.GetComponent<PlayerInventoryHolder>();
        
        if (playerInv != null)
        {
            OnShopWindowRequested?.Invoke(_shopSystem, playerInv);
            interactSuccesful = true;
        }
        else
        {
            interactSuccesful = false;
            Debug.Log("PlayerInventoryHolder not found");
        }
    }

    public void EndInteracion()
    {
        throw new System.NotImplementedException();
    }

}

[System.Serializable]
public class ShopSaveData
{
    public ShopSystem ShopSystem;
    
    public ShopSaveData(ShopSystem shopSystem)
    {
        ShopSystem = shopSystem;
    }
    
    
}