using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

public class StarController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameController controller;

    public Text text;

    private void Awake() {
        SetText().GetAwaiter();
    }

    private async Task SetText () {
        var done = 0;
        foreach(var m in controller.missions) {
            if (m.IsComplete()) {
                done += 1;
            }
        }
        text.text = $"{done} / {controller.missions.Count}";
        await Task.Delay(1000);
        await SetText();
    }
}
