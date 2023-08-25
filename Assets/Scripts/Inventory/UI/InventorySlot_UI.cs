using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventorySlot_UI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private GameObject _slotHighlight;
    [SerializeField] private InventorySlot assignedInventorySlot;

    private Button button;

    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    public InventoryDisplay ParentDisplay { get; private set; }

    private void Awake()
    {
        ClearSlot();

        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUISlotClick);

        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
        _slotHighlight.SetActive(false);
    }

    public void Init(InventorySlot slot)
    {
        assignedInventorySlot = slot;
        UpdateUISlot(slot);
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.ItemData != null)
        {
            itemSprite.sprite = slot.ItemData.Icon;
            itemSprite.color = Color.white;
        }
        else
        {
            ClearSlot();
        }

        if (slot.StackSize > 1) itemCount.text = slot.StackSize.ToString();
        else itemCount.text = "";
    }

    public void UpdateUISlot()
    {
        if(assignedInventorySlot != null) UpdateUISlot(assignedInventorySlot);
    }

    public void ClearSlot()
    {
        assignedInventorySlot?.ClearSlot();
        
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";
    }
    
    public void MoveStackToInventory(InventorySystem invToMoveTo)
    {
        if (invToMoveTo.AddToInventory(assignedInventorySlot.ItemData, assignedInventorySlot.StackSize))
        {
            ClearSlot();
        }
    }

    public void OnUISlotClick()
    {
        ParentDisplay?.SlotClicked(this);
    }

    public void ToggleHighLight()
    {
        _slotHighlight.SetActive(!_slotHighlight.activeInHierarchy);
    }
}
