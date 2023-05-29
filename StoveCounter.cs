using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress {

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }

    public enum State {
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
                    UpdateBurned();
                    break;
            }
        }
    }

    // UpdateFrying()
    private void UpdateFrying() {
        fryingTimer += Time.deltaTime;


        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { 
            progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
        });

        if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
            // Fried
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

            state = State.Fried;
            burningTimer = 0f;
            burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { 
                state = state 
            });
        }
    }

    // UpdateFried()
    private void UpdateFried() {
        burningTimer += Time.deltaTime;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { 
            progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
        });

        if (burningTimer > burningRecipeSO.burningTimerMax) {
            // Fried
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

            state = State.Burned;

            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { 
                state = state 
            });
        }
    }

    // UpdateBurned()
    private void UpdateBurned() {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { 
            progressNormalized = 0f
        });
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

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { 
                        state = state 
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { 
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
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
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    //Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        // Ingredient was added to the plate
                        GetKitchenObject().DestroySelf();

                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { 
                        state = state 
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { 
                            progressNormalized = 0f
                        });
                    } // else Ingredient was not added to the plate, Do nothing
                }
            } else {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { 
                state = state 
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { 
                    progressNormalized = 0f
                });
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
