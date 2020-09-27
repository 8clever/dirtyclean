using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes {
    Cutscene,
    GameOver,
    Menu,
    Options,
    Win,
    Tasks,
    Shop,
    Loading,
    SelectLevel,

    // levels
    endlessMode,
    lvl1,
    Lvl2
}

public class LoadSceneButton : MonoBehaviour
{
    public Scenes scene;

    public bool Async = false;

    [Header("Required only for LoadScene")]
    public LoadSceneMode mode;

    public void OnClick () {
        if (Async) {
            SceneManager.LoadSceneAsync(scene.ToString(), mode);
            return;
        }
        SceneManager.LoadScene(scene.ToString(), mode);
    }

    public void LoadScene () {
        OnClick();
    }

    public void UnloadScene () {
        SceneManager.UnloadSceneAsync(scene.ToString());
    }
}
