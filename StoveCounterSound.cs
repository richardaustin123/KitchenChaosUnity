using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour {

    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;
    private float warningSoundTimer = 0.5f;
    private bool playWarningSound;

    // Awake()
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    // Start()
    private void Start() {
        stoveCounter.OnStateChanged     += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged  += StoveCounter_OnProgressChanged;
    }

    // StoveCounter_OnProgressChanged()
    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        float burnShowProgressAmount = 0.5f;
        playWarningSound = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;
    }

    // StoveCounter_OnStateChanged()
    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;

        if (playSound) {
            audioSource.Play();
        } else {
            audioSource.Pause();
        }
    }

    // Update()
    private void Update() {
        if (playWarningSound) {
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer < 0f) {
                float warningSoundTimerMax = 0.2f;
                warningSoundTimer = warningSoundTimerMax;

                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }
}
