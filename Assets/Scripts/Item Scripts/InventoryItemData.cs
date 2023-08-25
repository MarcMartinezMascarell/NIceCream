using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public int ID = -1;
    public string DisplayName;
    [TextArea(4, 4)] public string Description;
    public Sprite Icon;
    public int MaxStackSize;
    public float Weight;
    public float DefaultPrice;
    public bool IsSellable;
    public bool IsConsumable;
    public GameObject ItemPrefab;
    public AudioClip PickUpSoundEffect = null;

    public void ConsumeItem()
    {
        if(IsConsumable)
        {
            Debug.Log($"Consumed {DisplayName}");
        }
    }
}
