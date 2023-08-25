using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory
{
    public class HotbarDisplay : StaticInventoryDisplay
    {
        private int _maxIndexSize = 9;
        private int _currentIndex = 0;
        
        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = new PlayerInput();
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _playerInput.Enable();

            _playerInput.Player.Hotbar1.performed += Hotbar1;
            _playerInput.Player.Hotbar2.performed += Hotbar2;
            _playerInput.Player.Hotbar3.performed += Hotbar3;
            _playerInput.Player.Hotbar4.performed += Hotbar4;
            _playerInput.Player.Hotbar5.performed += Hotbar5;
            _playerInput.Player.Hotbar6.performed += Hotbar6;
            _playerInput.Player.Hotbar7.performed += Hotbar7;
            _playerInput.Player.Hotbar8.performed += Hotbar8;
            _playerInput.Player.Hotbar9.performed += Hotbar9;
            _playerInput.Player.Hotbar10.performed += Hotbar10;
            _playerInput.Player.UseItem.performed += UseItem;
            
        }


        protected override void OnDisable()
        {
            base.OnDisable();
            _playerInput.Disable();
            
            _playerInput.Player.Hotbar1.performed -= Hotbar1;
            _playerInput.Player.Hotbar2.performed -= Hotbar2;
            _playerInput.Player.Hotbar3.performed -= Hotbar3;
            _playerInput.Player.Hotbar4.performed -= Hotbar4;
            _playerInput.Player.Hotbar5.performed -= Hotbar5;
            _playerInput.Player.Hotbar6.performed -= Hotbar6;
            _playerInput.Player.Hotbar7.performed -= Hotbar7;
            _playerInput.Player.Hotbar8.performed -= Hotbar8;
            _playerInput.Player.Hotbar9.performed -= Hotbar9;
            _playerInput.Player.Hotbar10.performed -= Hotbar10;
            _playerInput.Player.UseItem.performed -= UseItem;
        }

        protected override void Start()
        {
            base.Start();
            
            _currentIndex = 0;
            _maxIndexSize = slots.Length - 1;
            
            slots[_currentIndex].ToggleHighLight();
        }

        #region Hotbar Select Methods
        private void Hotbar1(InputAction.CallbackContext obj)
        {
            SetIndex(0);
            Debug.Log("Hotbar 1");
        }
        
        private void Hotbar2(InputAction.CallbackContext obj)
        {
            SetIndex(1);
        }
        
        private void Hotbar3(InputAction.CallbackContext obj)
        {
            SetIndex(2);
        }
        
        private void Hotbar4(InputAction.CallbackContext obj)
        {
            SetIndex(3);
        }
        
        private void Hotbar5(InputAction.CallbackContext obj)
        {
            SetIndex(4);
        }
        
        private void Hotbar6(InputAction.CallbackContext obj)
        {
            SetIndex(5);
        }
        
        private void Hotbar7(InputAction.CallbackContext obj)
        {
            SetIndex(6);
        }
        
        private void Hotbar8(InputAction.CallbackContext obj)
        {
            SetIndex(7);
        }
        
        private void Hotbar9(InputAction.CallbackContext obj)
        {
            SetIndex(8);
        }
        
        private void Hotbar10(InputAction.CallbackContext obj)
        {
            SetIndex(9);
        }
        #endregion

        private void Update()
        {
            if (_playerInput.Player.MouseWheel.ReadValue<float>() > 0.1f) ChangeIndex(-1);
            else if (_playerInput.Player.MouseWheel.ReadValue<float>() < -0.1f) ChangeIndex(1);
        }
        
        private void UseItem(InputAction.CallbackContext obj)
        {
            if(slots[_currentIndex].AssignedInventorySlot.ItemData != null && slots[_currentIndex].AssignedInventorySlot.ItemData.IsConsumable)
                slots[_currentIndex].AssignedInventorySlot.ItemData.ConsumeItem();
        }

        private void ChangeIndex(int direction)
        {
            slots[_currentIndex].ToggleHighLight();
            _currentIndex += direction;
            
            if (_currentIndex > _maxIndexSize) _currentIndex = 0;
            if (_currentIndex < 0) _currentIndex = _maxIndexSize;
            
            slots[_currentIndex].ToggleHighLight();
        }
        
        private void SetIndex(int index)
        {
            slots[_currentIndex].ToggleHighLight();
            if(index < 0) index = 0;
            if(index > _maxIndexSize) index = _maxIndexSize;
            
            _currentIndex = index;
            slots[_currentIndex].ToggleHighLight();
        }
    }
}