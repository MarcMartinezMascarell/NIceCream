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
    private CraftingRecipe _currentRecipe;
    
    public static UnityAction EndAllInteractions { get; set; }
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
            if(_currentRecipe != null)
            {
                playerInv.PrimaryInventorySystem.AddToInventory(_currentRecipe.craftedItems[0].item, _currentRecipe.craftedItems[0].amount);
                _currentRecipe = null;
                _craftingCanvas.gameObject.SetActive(false);
                interactSuccesful = true;
                EndInteraction();
            }
            else
            {
                OnCraftingWindowRequested?.Invoke(_craftingSystem, playerInv, this);
                interactSuccesful = true;
            }
        }
        else
        {
            interactSuccesful = false;
            Debug.Log("PlayerInventoryHolder not found");
        }
    }
    
    public void EndInteraction()
    {
        EndAllInteractions?.Invoke();
        OnInteractionComplete?.Invoke(this);
    }

    public void CraftRecipe(CraftingRecipe selectedRecipe)
    {
        _selectedRecipe = selectedRecipe;
        if(_selectedRecipe == null) return;
        if(_selectedRecipe.craftTime == 0)
        {
            ItemCrafted();
            return;
        }
        StartCoroutine(StartCraftingTimer());
    }
    
    public void ItemCrafted()
    {
        // _craftingCanvas.gameObject.SetActive(false);
        _craftingTimerImage.gameObject.SetActive(false);
        _itemPreviewImage.color = Color.white;
        _currentRecipe = _selectedRecipe;
    }
    
    private IEnumerator StartCraftingTimer()
    {
        var time = _selectedRecipe.craftTime;
        _craftingCanvas.gameObject.SetActive(true);
        _craftingTimerImage.gameObject.SetActive(true);
        _craftingTimerImageFill.gameObject.SetActive(true);

        while (time > 0)
        {
            _craftingTimerImageFill.fillAmount = Mathf.InverseLerp(_selectedRecipe.craftTime, 0, time );
            time--;
            yield return new WaitForSeconds(1f);
        }
        
        ItemCrafted();
    }
}
