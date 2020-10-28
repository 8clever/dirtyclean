using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    private void Update () {
        if (audioSource) {
            var config = Config.GetConfig();
            var cutscene = SceneManager.GetSceneByName(Scenes.Cutscene.ToString());
            if (cutscene.IsValid()) {
                audioSource.mute = true;
                return;
            }
            var mute = !config.Music;
            if (audioSource.mute != mute) {
                audioSource.mute = mute;
                if (config.Music) {
                    audioSource.time = 0f;
                }
            }
        }
    }
    public static async void PlayOnce (AudioClip clip) {
        var scene = SceneManager.GetActiveScene();
        var listener = scene.GetRootGameObjects()[0];
        var source = listener.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        var delay = System.Convert.ToInt32(clip.length * 1000);
        await System.Threading.Tasks.Task.Delay(delay);
        Destroy(source);
    }
}
