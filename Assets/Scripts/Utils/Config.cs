using System;
using System.IO;
using UnityEngine;


[Serializable]
public class Config {
    public class Layers
    {
        public int items = -1;
        public int nips = -1;
        public int dragged = -2;
    }

    public class Nip {
        public float moveSpeed = 1f;
    }
    private static string configPath = "/config.json";
    
    public Layers layers = new Layers();

    public Nip nip = new Nip();

    public bool GameFieldWeb = true;

    public bool Music = true;

    public int healthPointsAtSeconds = 15; 

    public int maxHealth = 300;

    public bool ShowCutsceneOnStart = true;

    public static Config GetConfig () {
        var path = Application.persistentDataPath + configPath;
        try {
            var json = File.ReadAllText(path);
            return JsonUtility.FromJson<Config>(json);
        } catch (Exception) {
            return new Config();
        }
    }

    public void PersistConfig () {
        var json = JsonUtility.ToJson(this);
        var path = Application.persistentDataPath + configPath;
        File.WriteAllText(path, json);
    }
}