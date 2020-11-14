using System;
using System.IO;
using UnityEngine;


[Serializable]
public class Config {
    [Serializable]
    public class Layers
    {
        public int items = -1;
        public int nips = -1;
        public int dragged = -2;
    }
    [Serializable]
    public class Nip {
        public float moveSpeed = 1f;
    }
    public Layers layers = new Layers();
    public Nip nip = new Nip();
    public bool HideCells = false;
    public bool Music = true;
    public int healthPointsAtSeconds = 15; 
    public bool ShowCutscene = true;
    private static string configKey = "configKey";
    private static Config config;
    public static Config GetConfig () {
        if (config == null) {
            var json = PlayerPrefs.GetString(configKey);
            config = JsonUtility.FromJson<Config>(json) ?? new Config();
        }
        return config;
    }

    public void PersistConfig () {
        config = this;
        var json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(configKey, json);
    }
}