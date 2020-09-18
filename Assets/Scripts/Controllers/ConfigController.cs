
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConfigController : MonoBehaviour {

    private Config config;

    public List<Toggle> toggles = new List<Toggle>();

    private void Awake() {
        config = Config.GetConfig();
        foreach (var t in toggles) {
            t.isOn = GetValue(t);
            t.onValueChanged.AddListener(delegate {
                OnChange(t);
            });
        }
    }

    public void OnChange (Toggle toggle) {
        config.GetType().GetField(toggle.name).SetValue(config, toggle.isOn);
        config.PersistConfig();
    }

    public bool GetValue (Toggle toggle) {
        return System.Convert.ToBoolean(config.GetType().GetField(toggle.name).GetValue(config));
    }
}