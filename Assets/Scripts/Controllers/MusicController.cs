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
            audioSource.mute = !config.Music;
        }
    }
}
