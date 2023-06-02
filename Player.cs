using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent {

    public static Player Instance { get; set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInterectDirection;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    // Awake()
    private void Awake() {
        if (Instance != null) {
            Debug.LogError("More than one Player insance");
        }
        Instance = this;    
    }

    // Start()
    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    // GameInput_OnInteractAction()
    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    // GameInput_OnInteractAlternateAction()
    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }
    }

    // Update()
    // Called once per frame by Unity
    private void Update() {
        HandleMovement();
        HandleInteraction();
    }


    // IsWalking()
    // Returns true if the player is walking
    public bool IsWalking() {
        return isWalking;
    }

    // HandleInteraction()
    private void HandleInteraction() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        
        if (moveDirection != Vector3.zero) {
            lastInterectDirection = moveDirection;
        } else {
            moveDirection = lastInterectDirection;
        }

        float interactDistance = 2f;

        if (Physics.Raycast(transform.position, lastInterectDirection, out RaycastHit raycastHit, interactDistance, countersLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                // raycast hit a baseCounter (Has baseCounter)
                // baseCounter.Interact();
                if (baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                }
            } else {
                // raycast hit something else
                SetSelectedCounter(null);
            } 
        } else {
            // raycast did not hit anything
            SetSelectedCounter(null);
        }
    }

    // HandleMovement()
    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        float playerRadius = 0.7f;
        float playerHeight = 1.8f;
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);
        
        if (!canMove) {
            // cannot move towards moveDirection
            // Attempt only x movement
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0f, 0f).normalized;
            canMove = (moveDirection.x != 0) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);

            if (canMove) {
                // an move only on the x axis
                moveDirection = moveDirectionX;
            } else {
                //Attempt only Z movement
                Vector3 moveDirectionZ = new Vector3(0f, 0f, moveDirection.z).normalized;
                canMove = (moveDirection.z != 0) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);

                if (canMove) {
                    // an move only on the Z axis
                    moveDirection = moveDirectionZ;
                } else {
                    // cannot move on either axis
                    // moveDirection = Vector3.zero;
                }
            }
        }

        if (canMove) {
            transform.position += moveDirection * moveDistance;
        }

        isWalking = moveDirection != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    // SetSelectedCounter()
    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { 
            selectedCounter = selectedCounter 
        });
    }

    // GetKitchenObjectFollowTransform()
    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    // SetKitchenObject()
    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
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

