using SaveLoadSystem;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Inventory
{
    public class PlayerInventoryHolder : InventoryHolder
    {
        public static UnityAction OnPlayerInventoryChange;
        public static UnityAction<InventorySystem, int> OnPlayerInventoryDisplayRequested;
        
        public NotificationsController notificationsController;

        private void Start()
        {
            notificationsController = FindObjectOfType<NotificationsController>();
            SaveGameManager.data.playerInventory = new InventorySaveData(primaryInventorySystem);
        }

        protected override void LoadInventory(SaveData data)
        {
            //Check the save data for this specific chest inventory, and if exists, load it in.
            if (data.playerInventory.inventorySystem != null)
            {
                this.primaryInventorySystem = data.playerInventory.inventorySystem;
                OnPlayerInventoryChange?.Invoke();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Keyboard.current.tabKey.wasPressedThisFrame)
            {
                OnPlayerInventoryDisplayRequested?.Invoke(primaryInventorySystem, offset);
            }
        }

        public bool AddToInventory(InventoryItemData data, int amount)
        {
            if (primaryInventorySystem.AddToInventory(data, amount))
            {
                notificationsController.DisplayNotification(data.Icon, data.DisplayName,  $"x{amount}");
                return true;
            }

            return false;
        }
    }
}
