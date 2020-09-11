using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadSceneButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Scenes scene;

    public void OnClick () {
        SceneManager.UnloadSceneAsync(scene.ToString());
    }
}
