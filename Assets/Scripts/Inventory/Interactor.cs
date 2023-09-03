using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public Transform InteractionPoint;
    public LayerMask InteracionLayer;
    public float InteractionPointRadius = 1f;
    public bool isInteracting { get; private set; }

    private void Update()
    {
        
        var colliders = Physics2D.OverlapCircleAll(InteractionPoint.position, InteractionPointRadius, InteracionLayer);
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                var interactable = colliders[i].GetComponent<IInteractable>();

                if (interactable != null) StartInteraction(interactable);
            }
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.tabKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame)
        {
            EndInteraction();
        }
        
    }

    void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccesful);
        isInteracting = true;
    }

    void EndInteraction()
    {
        isInteracting = false;
    }
}