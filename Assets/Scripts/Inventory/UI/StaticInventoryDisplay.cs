using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{

    [SerializeField] private InventoryHolder _inventoryHolder;
    [SerializeField] protected InventorySlot_UI[] slots;

    protected virtual void OnEnable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChange += RefreshStaticDisplay;
    }

    protected virtual void OnDisable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChange -= RefreshStaticDisplay;
    }

    private void RefreshStaticDisplay()
    {
        if (_inventoryHolder != null)
        {
            inventorySystem = _inventoryHolder.PrimaryInventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else
        {
            Debug.Log($"Warning! No inventory assigned to {this.gameObject}");
        }
        
        AssignSlot(inventorySystem, 0);
    }

    protected override void Start()
    {
        base.Start();
        RefreshStaticDisplay();
    }
    
    public override void AssignSlot(InventorySystem invToDisplay, int offset)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        for (int i = 0; i < _inventoryHolder.Offset; i++)
        {
            slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
        }
    }
    
}