using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class MenuController : MonoBehaviour
{

    public GameObject continueButton;
    public AudioSource audioSource;
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

    }

    private void Update () {
        config = Config.GetConfig();
        if (config.ShowCutscene) {
            config.ShowCutscene = false;
            config.PersistConfig();
            SceneManager.LoadScene(Scenes.Cutscene.ToString(), LoadSceneMode.Additive);
        }
    }
    public async void OnClickContinue () {
        await Task.Delay(LoadSceneButton.Delay);
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
