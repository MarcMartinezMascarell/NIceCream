using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueId))]
public class CraftingKeeper : MonoBehaviour, IInteractable
{
    [SerializeField] private CraftingRecipesList _craftingRecipesList;
    [SerializeField] private CraftingSystem _craftingSystem;

    private string _id;
    public static UnityAction<CraftingSystem, PlayerInventoryHolder> OnCraftingWindowRequested { get; set; }
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
            Debug.Log(_craftingSystem);
            OnCraftingWindowRequested?.Invoke(_craftingSystem, playerInv);
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
}
