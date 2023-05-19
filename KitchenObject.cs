using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour {
    
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO() {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {
        // If there is already a clear counter, clear it
        if (this.kitchenObjectParent != null) {
            this.kitchenObjectParent.ClearKitchenObject(); // Clear the previous kitchen object before assigning a new one
        }

        // Set the NEW kitchen object parent
        this.kitchenObjectParent = kitchenObjectParent;
        
        if (kitchenObjectParent.HasKitchenObject()) {
            Debug.LogError("IKitchenObjextParent already has a kitchen object");
        }

        kitchenObjectParent.SetKitchenObject(this);

        // Update the visual
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetkitchenObjectParent() {
        return kitchenObjectParent;
    }
}
