using System;
using System.Collections.Generic;
using Inventory.Inventory_Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private MouseItemData mouseInventoryItem;
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;
    
    [SerializeField] private DynamicInventoryDisplay dynamicInventory;
    [SerializeField] private DynamicInventoryDisplay playerBackpack;

    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;

    protected virtual void Start()
    {
        
    }

    public abstract void AssignSlot(InventorySystem invToDisplay, int offset);

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot)
            {
                slot.Key.UpdateUISlot(updatedSlot);
            }
        }
    }

    public void SlotClicked(InventorySlot_UI clickedUISlot)
    {
        bool isCtrl = Keyboard.current.leftCtrlKey.isPressed;
        bool isShift = Keyboard.current.leftShiftKey.isPressed;

        //Clicked slot has an item & mouse doesnt - Pick item
        if (clickedUISlot.AssignedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData == null)
        {
            //If the player is holding Shift - Move the entire stack to any open inventory
            if (isShift)
            {
                if (clickedUISlot.ParentDisplay == dynamicInventory)
                {
                    if (playerBackpack.gameObject.activeInHierarchy)
                    {
                        if(playerBackpack.InventorySystem.AddToInventory(clickedUISlot.AssignedInventorySlot.ItemData,
                            clickedUISlot.AssignedInventorySlot.StackSize))
                        {
                            clickedUISlot.ClearSlot();
                            return;
                        }
                    } 
                } else if(clickedUISlot.ParentDisplay == playerBackpack)
                {
                    if (dynamicInventory.gameObject.activeInHierarchy)
                    {
                        if(dynamicInventory.InventorySystem.AddToInventory(clickedUISlot.AssignedInventorySlot.ItemData,
                            clickedUISlot.AssignedInventorySlot.StackSize))
                        {
                            clickedUISlot.ClearSlot();
                            return;
                        }
                    }
                    // else if(playerBackpack.gameObject.activeInHierarchy)
                    // {
                    //     if(playerBackpack.InventorySystem.AddToInventory(clickedUISlot.AssignedInventorySlot.ItemData,
                    //         clickedUISlot.AssignedInventorySlot.StackSize))
                    //     {
                    //         clickedUISlot.ClearSlot();
                    //         return;
                    //     }
                    // }
                } else if(clickedUISlot.ParentDisplay.name == "HotBar Player")
                {
                    if (dynamicInventory.gameObject.activeInHierarchy)
                    {
                        if(dynamicInventory.InventorySystem.AddToInventory(clickedUISlot.AssignedInventorySlot.ItemData,
                            clickedUISlot.AssignedInventorySlot.StackSize))
                        {
                            clickedUISlot.ClearSlot();
                            return;
                        }
                    } else if(playerBackpack.gameObject.activeInHierarchy)
                    {
                        if(playerBackpack.InventorySystem.AddToInventory(clickedUISlot.AssignedInventorySlot.ItemData,
                               clickedUISlot.AssignedInventorySlot.StackSize, 10))
                        {
                            clickedUISlot.ClearSlot();
                            return;
                        }
                    }
                }
            }

            //If the player is holding Ctrl - Split stack
            if (isCtrl && clickedUISlot.AssignedInventorySlot.SplitStack(out InventorySlot halfStackSlot))
            {
                mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                clickedUISlot.UpdateUISlot();
                return;
            }
            else
            {
                mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
                clickedUISlot.ClearSlot();
                return;
            }
        }
        
        
        //Clicked slot doesnt have an item & mouse has - Place mouse item into the empty slot
        if (clickedUISlot.AssignedInventorySlot.ItemData == null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
            clickedUISlot.UpdateUISlot();
            
            mouseInventoryItem.ClearSlot();
            return;
        }
        //Both items are the same - Combine the stacks
                //If the mouse stack size + mouse stack size > slot max size, take from mouse
            //If different items, swap the items
            
        //Both slots have an item
        if (clickedUISlot.AssignedInventorySlot.ItemData != null &&
            mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemData ==
                              mouseInventoryItem.AssignedInventorySlot.ItemData;
            if (isSameItem &&
                clickedUISlot.AssignedInventorySlot.RoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize))
            {
                clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                clickedUISlot.UpdateUISlot();
                mouseInventoryItem.ClearSlot();
                return;
            }
            else if (isSameItem &&
                       !clickedUISlot.AssignedInventorySlot.RoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize, out int leftInStack))
            {
                if(leftInStack < 1) SwapSlots(clickedUISlot);
                else
                {
                    int remainingOnMouse = mouseInventoryItem.AssignedInventorySlot.StackSize - leftInStack;
                    clickedUISlot.AssignedInventorySlot.AddToStack(leftInStack);
                    clickedUISlot.UpdateUISlot();

                    var newItem = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData,
                        remainingOnMouse);
                    mouseInventoryItem.ClearSlot();
                    mouseInventoryItem.UpdateMouseSlot(newItem);
                    return;
                }
            }
            else if (!isSameItem)
            {
                SwapSlots(clickedUISlot);
                return;
            }

            return;
        }
        
    }

    private void SwapSlots(InventorySlot_UI clickedUISlot)
    {
        var clonedSlot = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData,
            mouseInventoryItem.AssignedInventorySlot.StackSize);
        mouseInventoryItem.ClearSlot();
        
        mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
        clickedUISlot.ClearSlot();
        
        clickedUISlot.AssignedInventorySlot.AssignItem(clonedSlot);
        clickedUISlot.UpdateUISlot();
    }
}
