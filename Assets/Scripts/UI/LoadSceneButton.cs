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
    public static int Delay = 150;
    public async void OnClick () {
        await Task.Delay(Delay); 
        if (Async) {
            SceneManager.LoadSceneAsync(scene.ToString(), mode);
            return;
        }
        SceneManager.LoadScene(scene.ToString(), mode);
    }

    public void LoadScene () {
        OnClick();
    }

    public async void UnloadScene () {
        await Task.Delay(Delay);
        SceneManager.UnloadSceneAsync(scene.ToString());
    }
}
