using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController
{

    public static string MenuScene = "Menu";

    public static string GameOverScene = "GameOver";

    public static string[] levels = {
        "lvl1"
    };

    public static bool isPause = false;

    public static void LoadScene (string SceneName) {
        isPause = false;
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }

    public static void TogglePause () {
        if (isPause) {
            isPause = false;
            SceneManager.UnloadSceneAsync(MenuScene);
            return;
        }
        isPause = true;
        SceneManager.LoadScene(MenuScene, LoadSceneMode.Additive);
    }
}
