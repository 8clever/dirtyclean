
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConfigController : MonoBehaviour {

    private Config config;

    public enum Type {
        Bool
    }

    [System.Serializable]
    public class ConfigField {
        public string Field;
        public string Label;
        public Type Type;
    }

    [System.Serializable]
    public class TypedInput {
        public Type Type;
        public GameObject Input;
    }

    public List<ConfigField> Fields;
    public List<TypedInput> Inputs;
    public GameObject Container;
    private void Awake() {
        config = Config.GetConfig();
        foreach (var f in Fields) {
            if (f.Type == Type.Bool) {
                var input = Inputs.Find(i => i.Type == f.Type);
                var checkbox = Instantiate(input.Input.GetComponent<Checkbox>(), Container.transform);
                var btn = checkbox.GetComponent<Button>();
                var value = GetValue(f);
                checkbox.text = f.Label;
                checkbox.Checked = System.Convert.ToBoolean(value);
                checkbox.Underline = false;
                btn?.onClick.AddListener(delegate {
                    checkbox.Checked = !System.Convert.ToBoolean(GetValue(f));
                    OnChange(f, checkbox.Checked);
                });
            }
        }
    }

    public void OnChange (ConfigField field, object value) {
        config.GetType().GetField(field.Field).SetValue(config, value);
        config.PersistConfig();
    }

    public object GetValue (ConfigField field) {
        return config.GetType().GetField(field.Field).GetValue(config);
    }
}