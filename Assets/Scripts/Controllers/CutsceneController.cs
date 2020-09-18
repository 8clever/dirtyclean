using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{

    private Config config;

    private void Awake() {
        config = Config.GetConfig();
    }
    public void OnEnd () {
        config.ShowCutsceneOnStart = false;
        config.PersistConfig();
    }    
}
