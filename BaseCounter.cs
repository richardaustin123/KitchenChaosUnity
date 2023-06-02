using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent {    
    
    public static event EventHandler OnAnyObjectPlacedHere;

    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    // Interact()
    public virtual void Interact(Player player) {
        Debug.Log("Interact() called on BaseCounter");
    }

    // InteractAlternate()
    public virtual void InteractAlternate(Player player) {
        // Debug.Log("InteractAlternate() called on BaseCounter");
    }

    // GetKitchenObjectFollowTransform()
    public Transform GetKitchenObjectFollowTransform() {
        return counterTopPoint;
    }

    // SetKitchenObject()
    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

    // GetKitchenObject()
    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    // ClearKitchenObject()
    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    // HasKitchenObject()
    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}
