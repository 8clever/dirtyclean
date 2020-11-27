using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

public class StarController : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text;

    private void Awake() {
        text.text = "0";
        SetText().GetAwaiter();
    }

    private async Task SetText () {
        var controller = FindObjectOfType<GameController>();
        text.text = controller.stars.ToString();
        await Task.Delay(1000);
        await SetText();
    }
}
