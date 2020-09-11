using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public GameObject continueButton;

    private Toggle gameFieldWeb;

    private Config config;

    private void Awake() {
        config = Config.GetConfig();

        if (continueButton) {
            continueButton.SetActive(SceneManager.GetActiveScene().name != Scenes.Menu.ToString());
        }
    }

    private void Start () {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void SceneLoaded (Scene scene, LoadSceneMode mode) {
        if (mode == LoadSceneMode.Additive) {
            var objects = scene.GetRootGameObjects();
            foreach (var obj in objects) {
                var audioSource = obj.GetComponentInChildren<AudioSource>();
                var audioListener = obj.GetComponentInChildren<AudioListener>();
                Destroy(audioSource);
                Destroy(audioListener);
            }
        }
    }
}
