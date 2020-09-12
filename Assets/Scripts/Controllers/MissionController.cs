using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionController : MonoBehaviour
{
    public Mission[] missions = {
        new Mission("Test Mission")
    };

    private GameObject content;
    public void Start()
    {
        var scene = SceneManager.GetActiveScene();
        var objects = scene.GetRootGameObjects();
        foreach (var o in objects) {
            var controller = o.GetComponent<GameController>();
            if (controller) {
                missions = controller.missions.ToArray();
            }
        }
        content = GameObject.Find("Viewport/Content");
        var n = 1;
        foreach(var m in missions) {
            var obj = Instantiate(Resources.Load<GameObject>("UI/MissionText"), content.transform);
            var text = obj.GetComponent<Text>();
            text.text = $"{n}. {m.name}";
            n += 1;
        }
    }
    void Update()
    {
        
    }
}
