using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour {

    public static KitchenGameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    // Enums
    private enum State {
        WaitingToStart,
        CoutndownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 30f;
    private bool isGamePaused = false;

    // Awake()
    private void Awake() {
        Instance = this;
        state = State.WaitingToStart;
    }

    // Start()
    private void Start() {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    // GameInput_OnPauseAction()
    private void GameInput_OnPauseAction(object sender, EventArgs e) {
        TogglePauseGame();
    }

    // GameInput_OnInteractAction()
    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (state == State.WaitingToStart) {
            state = State.CoutndownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    // Update()
    private void Update() {
        switch (state) {
            case State.WaitingToStart:
                break;
            case State.CoutndownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f) {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f) {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }

    }

    // IsGamePlaying()
    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }

    // IsCountdownToStartActive()
    public bool IsCountdownToStartActive() {
        return state == State.CoutndownToStart;
    }

    // GetCountdownToStartTimer()
    public float GetCountdownToStartTimer() {
        return countdownToStartTimer;
    }

    // IsGameOver()
    public bool IsGameOver() {
        return state == State.GameOver;
    }

    // GetGamePlayingTimerNormalized()
    public float GetGamePlayingTimerNormalized() {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    // TogglePauseGame()
    public void TogglePauseGame() {
        isGamePaused = !isGamePaused;

        if (isGamePaused) {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        } else {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

}
