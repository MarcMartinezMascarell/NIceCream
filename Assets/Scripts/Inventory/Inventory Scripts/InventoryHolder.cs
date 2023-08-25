using SaveLoadSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Inventory
{
    [System.Serializable]
    public abstract class InventoryHolder : MonoBehaviour
    {
        [SerializeField] private int inventorySize;
        [SerializeField] protected InventorySystem primaryInventorySystem;
        [SerializeField] protected int offset = 10;
        [SerializeField] protected float _money;
    
        public int Offset => offset;

        public InventorySystem PrimaryInventorySystem => primaryInventorySystem;
    
        public static UnityAction<InventorySystem, int> OnDynamicInventoryDisplayRequested; //Inv system to display, amount to offset display by

        protected virtual void Awake()
        {
            SaveLoad.OnLoadGame += LoadInventory;
        
            primaryInventorySystem = new InventorySystem(inventorySize, _money);
        }

        protected abstract void LoadInventory(SaveData data);
    }

    [System.Serializable]
    public struct InventorySaveData
    {
        public InventorySystem inventorySystem;
        public Vector3 position;
        public Quaternion rotation;
    
        public InventorySaveData(InventorySystem _inventorySystem, Vector3 _position, Quaternion _rotation)
        {
            inventorySystem = _inventorySystem;
            position = _position;
            rotation = _rotation;
        }
    
        public InventorySaveData(InventorySystem _inventorySystem)
        {
            inventorySystem = _inventorySystem;
            position = Vector3.zero;
            rotation = Quaternion.identity;
        }
    }
}