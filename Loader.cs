using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Loader
// Static = not attatched to any instance of a class
public static class Loader {

    // Enums
    public enum Scene {
        MainMenuScene,
        GameScene,
        LoadingScene,
    }

    private static Scene targetScene;
    
    // Load()
    public static void Load(Scene targetScene) {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    // LoaderCallback()
    public static void LoaderCallback() {
        SceneManager.LoadScene(targetScene.ToString());
    }

}
