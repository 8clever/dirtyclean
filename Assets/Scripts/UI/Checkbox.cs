using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Checkbox : MonoBehaviour {
    public Sprite CheckedSprite;
    public Sprite NotCheckedSprite;
    public Image Check;
    public TMPro.TMP_Text Text;
    public string text;
    public bool Checked;
    public bool Underline = true;
    public void Update () {
        Check.sprite = Checked ? CheckedSprite : NotCheckedSprite;
        Text.text = text;
        if (Checked && Underline) {
            Text.fontStyle = TMPro.FontStyles.Bold | TMPro.FontStyles.Strikethrough;
        }
    }
}