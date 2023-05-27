using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter {

    private enum State {
        Idle, 
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;

    // Start()
    private void Start() {
        state = State.Idle;
    }

    // Update()
    private void Update() {
        if (HasKitchenObject()) {
            switch (state) {
                case State.Idle:
                    // UpdateIdle();
                    break;
                case State.Frying: 
                    UpdateFrying();
                    break;
                case State.Fried:
                    UpdateFried();
                    break;
                case State.Burned:
                    // UpdateBurned();
                    break;
            }
            Debug.Log(state);
        }
    }

    // UpdateFrying()
    private void UpdateFrying() {
        fryingTimer += Time.deltaTime;
        if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
            // Fried
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

            state = State.Fried;
            burningTimer = 0f;
            burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
        }
    }

    // UpdateFried()
    private void UpdateFried() {
        burningTimer += Time.deltaTime;
        if (burningTimer > burningRecipeSO.burningTimerMax) {
            // Fried
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

            state = State.Burned;
        }
    }

    // Interact()
    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            //there is no kitchen object on the counter
            if (player.HasKitchenObject()) {
                // Player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    // Player is carrying something that can be fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;
                } else {
                    // Player is carrying something that cannot be fried
                    // Do nothing
                }
            } else {
                // Player is not carrying anything
                // Do nothing
            }
        } else {
            if (player.HasKitchenObject()) {
                // Player is carrying something
            } else {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    // HasRecipeWithInput()
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    // GetOutputForInput()
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null) {
            return fryingRecipeSO.output;
        } else {
            return null;
        }
    }

    // GetFryingRecipeSOWithInput()
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == inputKitchenObjectSO) {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    // GetBurningRecipeSOWithInput()
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            if (burningRecipeSO.input == inputKitchenObjectSO) {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
