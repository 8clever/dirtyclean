﻿using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes {
    Cutscene,
    GameOver,
    Menu,
    Options,
    Win,
    Tasks,
    Shop,
    Loading
}

public enum Levels {
    lvl1
}

public class LoadSceneButton : MonoBehaviour
{
    public Scenes scene;

    public LoadSceneMode mode;

    public void OnClick () {
        SceneManager.LoadScene(scene.ToString(), mode);
    }

    public void LoadScene () {
        OnClick();
    }

    public void UnloadScene () {
        SceneManager.UnloadSceneAsync(scene.ToString());
    }
}
