using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes {
    Cutscene,
    GameOver,
    Menu,
    Options,
    Win,
}

public enum Levels {
    lvl1
}

public class LoadSceneButton : MonoBehaviour
{
    public Scenes scene;

    public LoadSceneMode mode;

    public void OnClick () {
        SceneManager.LoadScene(scene.ToString(), mode);
    }
}
