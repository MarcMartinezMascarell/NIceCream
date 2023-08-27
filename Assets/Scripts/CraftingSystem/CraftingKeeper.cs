using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(UniqueId))]
public class CraftingKeeper : MonoBehaviour, IInteractable
{
    [SerializeField] private CraftingRecipesList _craftingRecipesList;
    [SerializeField] private CraftingSystem _craftingSystem;
    
    [Header("Crafting UI")]
    [SerializeField] private Canvas _craftingCanvas;
    [SerializeField] private Image _craftingTimerImage;
    [SerializeField] private Image _craftingTimerImageFill;
    [SerializeField] private Image _itemPreviewImage;

    private string _id;
    private CraftingRecipe _selectedRecipe;
    public static UnityAction<CraftingSystem, PlayerInventoryHolder, CraftingKeeper> OnCraftingWindowRequested { get; set; }
    private void Awake()
    {
        _craftingSystem = new CraftingSystem(_craftingSystem.CraftingName);
        
        _craftingSystem.AddCraftingRecipes(_craftingRecipesList);

        _id = GetComponent<UniqueId>().ID;
    }


    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    public void Interact(Interactor interactor, out bool interactSuccesful)
    {
        var playerInv = interactor.GetComponent<PlayerInventoryHolder>();
        
        if (playerInv != null)
        {
            OnCraftingWindowRequested?.Invoke(_craftingSystem, playerInv, this);
            interactSuccesful = true;
        }
        else
        {
            interactSuccesful = false;
            Debug.Log("PlayerInventoryHolder not found");
        }
    }
    
    public void EndInteracion()
    {
        throw new System.NotImplementedException();
    }

    public void CraftRecipe(CraftingRecipe selectedRecipe)
    {
        _selectedRecipe = selectedRecipe;
        StartCoroutine(StartCraftingTimer());
    }
    
    public void ItemCrafted()
    {
        _craftingCanvas.gameObject.SetActive(false);
        Debug.Log("Item Crafted");
    }
    
    private IEnumerator StartCraftingTimer()
    {
        var time = _selectedRecipe.craftTime;
        _craftingCanvas.gameObject.SetActive(true);

        while (time > 0)
        {
            _craftingTimerImageFill.fillAmount = Mathf.InverseLerp(_selectedRecipe.craftTime, 0, time );
            time--;
            yield return new WaitForSeconds(1f);
        }
        
        ItemCrafted();
    }
}
