using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter {

    // public event EventHandler<OnPlateSpawnedEventArgs> OnPlateSpawned;
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 2f;
    private int plateSpawnedAmount;
    private int plateSpawnedAmountMax = 4;

    private void Update() {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax) {
            spawnPlateTimer = 0f;

            if (KitchenGameManager.Instance.IsGamePlaying() && plateSpawnedAmount < plateSpawnedAmountMax) {
                plateSpawnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
                // OnPlateSpawned?.Invoke(this, new OnPlateSpawnedEventArgs(plateKitchenObjectSO));
                // SpawnPlate();
            }
        }
    }

    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {
            // Player is not carrying anything
            if (plateSpawnedAmount > 0) {
                // There is a plate on the counter
                // Pick up the plate
                plateSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            } else {
                // There is no plate on the counter
                // Do nothing
            }
        }
    }
}
