using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour {

    private PlayerInputActions playerInputActions;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;

    // Awake()
    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
    }

    // Interact_performed()
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
            OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    // InteractAlternate_performed()
    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
            OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    // GetMovementVectorNormalized()
    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}
