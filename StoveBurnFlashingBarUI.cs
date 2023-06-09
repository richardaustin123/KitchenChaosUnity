using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour {

    private const string IS_FLASHING = "IsFlashing";

    [SerializeField] private StoveCounter stoveCounter;

    private Animator animator;

    // Awake()
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    // Start()
    private void Start() {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        // Set flashing bar to false at start
        animator.SetBool(IS_FLASHING, false);
    }

    // StoveCounter_OnProgressChanged()
    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        float burnShowProgressAmount = 0.5f;
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        animator.SetBool(IS_FLASHING, show);
    }
}
