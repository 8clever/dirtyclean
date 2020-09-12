using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private Config config;

    public AudioSource audioSource;

    private void Start() {
        config = Config.GetConfig();
        audioSource.mute = !config.music;
    }
}
