using UnityEngine;
using UnityEngine.UI;

public class LibraryItem : MonoBehaviour {
    public Image GO_Image;
    public Text GO_Text;
    public void Set (Sprite Image, string Text) {
        GO_Image.sprite = Image;
        GO_Text.text = Text;
    }
}