using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Checkbox : MonoBehaviour {
    public GameObject GO_Checked;
    public GameObject GO_NotChecked;
    public TMPro.TMP_Text Text;
    public AudioClip ClickOnChecked;
    public AudioClip ClickOnNotChecked;
    public string text;
    public bool Checked;
    public bool Underline = true;

    private void Awake() {
        gameObject.AddComponent<AudioSource>();
    }

    public void Update () {
        Text.text = text;
        if (Checked && Underline) {
            Text.fontStyle = TMPro.FontStyles.Bold | TMPro.FontStyles.Strikethrough;
        }
        if (
            GO_Checked.activeSelf != GO_NotChecked.activeSelf &&
            GO_Checked.activeSelf != Checked
        ) {
            var source = GetComponent<AudioSource>();
            source.clip = Checked ? ClickOnChecked : ClickOnNotChecked;
            source.Play();
        }
        GO_Checked.SetActive(Checked);
        GO_NotChecked.SetActive(!Checked);
    }
}