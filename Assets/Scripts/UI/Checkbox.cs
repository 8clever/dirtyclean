using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Checkbox : MonoBehaviour {
    public GameObject GO_Checked;
    public GameObject GO_NotChecked;
    public TMPro.TMP_Text Text;
    public string text;
    public bool Checked;
    public bool Underline = true;
    public void Update () {
        GO_Checked.SetActive(Checked);
        GO_NotChecked.SetActive(!Checked);
        Text.text = text;
        if (Checked && Underline) {
            Text.fontStyle = TMPro.FontStyles.Bold | TMPro.FontStyles.Strikethrough;
        }
    }
}