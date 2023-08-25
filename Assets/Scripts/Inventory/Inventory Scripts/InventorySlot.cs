using UnityEngine;

namespace Inventory.Inventory_Scripts
{
    [System.Serializable]
    public class InventorySlot : ItemSlot
    {


        public InventorySlot(InventoryItemData source, int amount)
        {
            itemData = source;
            itemID = itemData.ID;
            stackSize = amount;
        }

        public InventorySlot()
        {
            ClearSlot();
        }

        public void UpdateInventorySlot(InventoryItemData data, int amount)
        {
            itemData = data;
            itemID = itemData.ID;
            stackSize = amount;
        }

        public bool RoomLeftInStack(int amountToAdd, out int amountRemaining)
        {
            amountRemaining = ItemData.MaxStackSize - stackSize;
            return RoomLeftInStack(amountToAdd);
        }

        public bool RoomLeftInStack(int amountToAdd)
        {
            if (stackSize + amountToAdd <= itemData.MaxStackSize) return true;
            else return false;
        }

        public bool SplitStack(out InventorySlot splitStack)
        {
            if (stackSize <= 1)
            {
                splitStack = null;
                return false;
            }

            int halfStack = Mathf.RoundToInt(stackSize / 2);
            RemoveFromStack(halfStack);

            splitStack = new InventorySlot(itemData, halfStack);
            return true;
        }
    }
}
