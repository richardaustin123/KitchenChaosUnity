using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    
    private const string IS_WALKING = "IsWalking";
    private Animator animator;
    
    [SerializeField] private Player player;

    // Awake()
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    // Update()
    void Update() {
        animator.SetBool(IS_WALKING, player.IsWalking());   
    }
}
