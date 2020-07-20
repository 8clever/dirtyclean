using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    GameObject continueButton;


    private void Awake() {
        continueButton = GameObject.Find("ContinueButton");
    }

    private void Start () {
        SceneManager.sceneLoaded += SceneLoaded;
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
        if (continueButton && continueButton.activeSelf != SceneController.isPause) {
            continueButton.SetActive(SceneController.isPause);
        }
    }

    public void NewGame () {
        SceneController.LoadScene(SceneController.levels[0]);
    }

    public void TogglePause () {
        SceneController.TogglePause();
        var audioSource = this.GetComponent<AudioSource>();
    }

    public void ReturnToMainMenu () {
        SceneController.LoadScene(SceneController.MenuScene);
    }
}
