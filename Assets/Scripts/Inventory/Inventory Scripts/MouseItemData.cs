using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseItemData : MonoBehaviour
{
    public Image ItemSprite;
    public TextMeshProUGUI ItemCount;
    public InventorySlot AssignedInventorySlot;
    [SerializeField] private float _dropDistance = 2f;
    
    private Transform _playerTransform;
    private Animator _playerAnimator;

    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemCount.text = "";
        
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _playerAnimator = _playerTransform.GetComponent<Animator>();
        
        if(_playerTransform == null) Debug.LogError("Player not found");
    }

    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        AssignedInventorySlot.AssignItem(invSlot);
        UpdateMouseSlot();
    }
    
    public void UpdateMouseSlot()
    {
        ItemSprite.sprite = AssignedInventorySlot.ItemData.Icon;
        ItemCount.text = AssignedInventorySlot.StackSize.ToString();
        ItemSprite.color = Color.white;
    }

    private void Update()
    {
        if (AssignedInventorySlot.ItemData != null)
        {
            transform.position = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                DropItemOnTheGround();
            }
        }
    }

    private void DropItemOnTheGround()
    {
        if (AssignedInventorySlot.ItemData.ItemPrefab != null)
        {
            var item = PrefabUtility.InstantiatePrefab(AssignedInventorySlot.ItemData.ItemPrefab) as GameObject;
            item.transform.position = _playerTransform.position;
            var itemPickUp = item.GetComponent<ItemPickUp>();
            itemPickUp.Throw(_playerAnimator.GetFloat("horizontal"));
        }

        if (AssignedInventorySlot.StackSize > 1)
        {
            AssignedInventorySlot.AddToStack(-1);
            UpdateMouseSlot();
        } else
            ClearSlot();
    }

    public void ClearSlot()
    {
        AssignedInventorySlot.ClearSlot();
        ItemCount.text = "";
        ItemSprite.color = Color.clear;
        ItemSprite.sprite = null;
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
