using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour {

    [SerializeField] private StoveCounter stoveCounter;

    // Start()
    private void Start() {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        Hide();
    }

    // StoveCounter_OnProgressChanged()
    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        float burnShowProgressAmount = 0.5f;
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        if (show) {
            Show();
        } else {
            Hide();
        }
    }

    // Show()
    private void Show() {
        gameObject.SetActive(true);
    }

    // Hide()
    private void Hide() {
        gameObject.SetActive(false);
    }

}
