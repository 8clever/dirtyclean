using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{

    GameObject continueButton;

    private Toggle gameFieldWebToggle;

    private void Awake() {
        continueButton = GameObject.Find("ContinueButton");
    }

    private void Start () {
        SceneManager.sceneLoaded += SceneLoaded;
        var gameFieldWebToggle = GameObject.Find("GameFieldWebToggle");
        if (gameFieldWebToggle) {
            this.gameFieldWebToggle = gameFieldWebToggle.GetComponent<Toggle>();
            this.gameFieldWebToggle.isOn = Config.gameFieldWeb;
        }
    }

    private void SceneLoaded (Scene scene, LoadSceneMode mode) {
        if (mode == LoadSceneMode.Additive) {
            var objects = scene.GetRootGameObjects();
            foreach (var obj in objects) {
                var audioSource = obj.GetComponentInChildren<AudioSource>();
                if (audioSource) {
                    audioSource.Pause();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameFieldWebToggle && gameFieldWebToggle.isOn != Config.gameFieldWeb) {
            Config.gameFieldWeb = gameFieldWebToggle.isOn;
        }

        if (continueButton && continueButton.activeSelf != SceneController.isPause) {
            continueButton.SetActive(SceneController.isPause);
        }
    }

    public void NewGame () {
        StartCoroutine(LoadScene(SceneController.levels[0], LoadSceneMode.Single));
    }

    public void TogglePause () {
        var scene = SceneManager.GetSceneByName(SceneController.MenuScene);
        if (scene.isLoaded) {
            SceneController.isPause = false;
            SceneManager.UnloadSceneAsync(SceneController.MenuScene);
            return;
        }
        SceneController.isPause = true;
        StartCoroutine(LoadScene(SceneController.MenuScene, LoadSceneMode.Additive));
    }

    public void ReturnToMainMenu () {
        SceneManager.LoadScene(SceneController.MenuScene);
    }

    IEnumerator LoadScene (string scene, LoadSceneMode mode) {
        yield return SceneManager.LoadSceneAsync(scene, mode);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
    }

    public void ToggelOptions () {
        var scene = SceneManager.GetSceneByName(SceneController.Options);
        if (scene.isLoaded) {
            SceneManager.UnloadSceneAsync(SceneController.Options);
            return;
        }
        StartCoroutine(LoadScene(SceneController.Options, LoadSceneMode.Additive));
    }
}
