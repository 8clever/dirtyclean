using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    void OnEndCutScene () {
        SceneManager.LoadScene(Scenes.lvl1.ToString());
    }
}
