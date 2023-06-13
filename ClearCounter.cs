using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // Interact()
    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            //there is no kitchen object on the counter
            if (player.HasKitchenObject()) {
                // Player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else {
                // Player is not carrying anything
                // Do nothing
            }
        } else {
            if (player.HasKitchenObject()) {
                // Player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    //Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        // Ingredient was added to the plate
                        GetKitchenObject().DestroySelf();
                    }    // else Ingredient was not added to the plate, Do nothing
                } else { // Player is carrying something that is not a plate
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        // Kitchen object on the counter is a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {
                            // Ingredient was added to the plate
                            player.GetKitchenObject().DestroySelf();
                        } 
                    }
                }
            } else { // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

}
