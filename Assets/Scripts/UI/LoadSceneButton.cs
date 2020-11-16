using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

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
    endlessMode,
    LVL1,
    LVL2,
    Library,
    LVL3,
    LVL4,
    NeedTurns
}

public class LoadSceneButton : MonoBehaviour
{
    public Scenes scene;
    public bool Async = false;

    [Header("Required only for LoadScene")]
    public LoadSceneMode mode;
    public AudioClip audioClick;
    public static readonly int Delay = 0;
    public void OnClick () {
        if (Async) {
            SceneManager.LoadSceneAsync(scene.ToString(), mode);
        } else {
            SceneManager.LoadScene(scene.ToString(), mode);
        }
        if (audioClick) {
            MusicController.PlayOnce(audioClick);
        }
    }

    public void LoadScene () {
        OnClick();
    }

    public void UnloadScene () {
        SceneManager.UnloadSceneAsync(scene.ToString());
        if (audioClick) {
            MusicController.PlayOnce(audioClick);
        }
    }
}
