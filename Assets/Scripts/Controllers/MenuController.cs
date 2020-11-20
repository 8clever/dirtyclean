using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject continueButton;
    public AudioSource audioSource;
    public AudioClip audioClickContinue;
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
        if (isAdditive && audioSource) {
            audioSource.Pause();
        }
    }
    private void Start() {
        Application.targetFrameRate = 60;
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
        if (audioClickContinue) {
            MusicController.PlayOnce(audioClickContinue);
        }
        if (isAdditive) {
            SceneManager.UnloadSceneAsync(Scenes.Menu.ToString());
            return;
        }
        if (hasSave) {
            GameController.LoadLevel();
            return;
        }
    }
}
