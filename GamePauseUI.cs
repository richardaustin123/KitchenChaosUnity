using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour {

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;

    private void Awake() {
        resumeButton.onClick.AddListener(() => {
            KitchenGameManager.Instance.TogglePauseGame();
        });

        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        optionsButton.onClick.AddListener(() => {
            OptionsUI.Instance.Show();
        });
    }

    // Start()
    private void Start() {
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        Hide();
    }

    // KitchenGameManager_OnGamePaused()
    private void KitchenGameManager_OnGamePaused(object sender, System.EventArgs e) {
        Show();
    }

    // KitchenGameManager_OnGameUnpaused() 
    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e) {
        Hide();
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
