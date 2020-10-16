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
    public bool GameFieldWeb = true;
    public bool Music = true;
    public int healthPointsAtSeconds = 15; 
    public int maxHealth = 300;
    public bool ShowCutscene = true;
    private static string configKey = "configKey";

    public static Config GetConfig () {
        var json = PlayerPrefs.GetString(configKey);
        return JsonUtility.FromJson<Config>(json) ?? new Config();
    }

    public void PersistConfig () {
        var json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(configKey, json);
    }
}