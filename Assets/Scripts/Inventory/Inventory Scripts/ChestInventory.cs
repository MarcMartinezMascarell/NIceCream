using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using SaveLoadSystem;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueId))]
public class ChestInventory : InventoryHolder, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    // protected override void Awake()
    // {
    //     base.Awake();
    //     SaveLoad.OnLoadGame += LoadInventory;
    // }

    private void Start()
    {
        var chestSaveData = new InventorySaveData(primaryInventorySystem, transform.position, transform.rotation);
        SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueId>().ID, chestSaveData);
    }

    protected override void LoadInventory(SaveData data)
    {
        //Check the save data for this specific chest inventory, and if exists, load it in.
        if (data.chestDictionary.TryGetValue(GetComponent<UniqueId>().ID, out InventorySaveData chestData))
        {
            this.primaryInventorySystem = chestData.inventorySystem;
            this.transform.position = chestData.position;
            this.transform.rotation = chestData.rotation;
        }
    }

    public void Interact(Interactor interactor, out bool interactSuccesful)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem, 0);
        interactSuccesful = true;
    }

    public void EndInteracion()
    {
        
    }
}