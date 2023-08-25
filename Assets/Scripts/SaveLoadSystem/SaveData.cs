using System.Collections.Generic;
using Inventory;
using Inventory.Inventory_Scripts;
using UnityEngine;

namespace SaveLoadSystem
{
    [System.Serializable]
    public class SaveData
    {
        public List<string> collectedItems;
        public SerializableDictionary<string, ItemPickUpSaveData> activeItems;
        public SerializableDictionary<string, InventorySaveData> chestDictionary;
        public InventorySaveData playerInventory;
        public SerializableDictionary<string, ShopSaveData> _shopKeeperDictonary;

        public SaveData()
        {
            collectedItems = new List<string>();
            activeItems = new SerializableDictionary<string, ItemPickUpSaveData>();
            chestDictionary = new SerializableDictionary<string, InventorySaveData>();
            playerInventory = new InventorySaveData();
            _shopKeeperDictonary = new SerializableDictionary<string, ShopSaveData>();
        }
    }
}
