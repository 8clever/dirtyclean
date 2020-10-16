using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public GameObject continueButton;

    private Toggle gameFieldWeb;

    private Config config;

    private bool hasSave;

    private bool isAdditive;
    private void Awake() {
        isAdditive = SceneManager.GetActiveScene().name != Scenes.Menu.ToString();
        hasSave = PlayerPrefs.HasKey(GameController.saveKey);
        if (continueButton) {
            continueButton.SetActive(isAdditive || hasSave);
        }
        
    }
    private void Start() {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void Update () {
        config = Config.GetConfig();
        if (config.ShowCutscene) {
            config.ShowCutscene = false;
            config.PersistConfig();
            SceneManager.LoadScene(Scenes.Cutscene.ToString(), LoadSceneMode.Additive);
        }
    }
    public void OnClickContinue () {
        if (isAdditive) {
            SceneManager.UnloadSceneAsync(Scenes.Menu.ToString());
            return;
        }
        if (hasSave) {
            GameController.LoadLevel();
            return;
        }
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
