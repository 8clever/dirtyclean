using UnityEngine;
using UnityEngine.UI;
using System;
public class CheckboxConfigController : MonoBehaviour
{

    public Toggle toggle;

    public string field;

    private void Awake() {
        var config = Config.GetConfig();
        var value = config.GetType().GetField(field).GetValue(config);
        toggle.isOn = Convert.ToBoolean(value);
        toggle.onValueChanged.AddListener(delegate {
            OnChange(toggle);
        });
        
    }

    void OnChange (Toggle toggle) {
        var config = Config.GetConfig();
        config.GetType().GetField(field).SetValue(config, toggle.isOn);
        config.PersistConfig();
    }
}
