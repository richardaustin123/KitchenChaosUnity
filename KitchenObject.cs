using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour {
    
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    // GetKitchenObjectSO()
    public KitchenObjectSO GetKitchenObjectSO() {
        return kitchenObjectSO;
    }

    // SetKitchenObjectParent()
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {
        // If there is already a clear counter, clear it
        if (this.kitchenObjectParent != null) {
            this.kitchenObjectParent.ClearKitchenObject(); // Clear the previous kitchen object before assigning a new one
        }

        // Set the NEW kitchen object parent
        this.kitchenObjectParent = kitchenObjectParent;
        
        if (kitchenObjectParent.HasKitchenObject()) {
            Debug.LogError("IKitchenObjectParent already has a kitchen object");
        }

        kitchenObjectParent.SetKitchenObject(this);

        // Update the visual
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    // GetkitchenObjectParent()
    public IKitchenObjectParent GetkitchenObjectParent() {
        return kitchenObjectParent;
    }

    // DestroySelf()
    public void DestroySelf() {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    // TryGetPlate()
    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject) {
        if (this is PlateKitchenObject) {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        } else {
            plateKitchenObject = null;
            return false;
        }
    }

    // SpawnKitchenObject()
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent) {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }
}
