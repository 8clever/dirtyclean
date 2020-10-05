using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionController : MonoBehaviour
{
    public Mission[] missions = {};
    public GameObject content;
    public Checkbox Checkbox;
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
        foreach(var m in missions) {
            var obj = Instantiate(Checkbox, content.transform);
            obj.text = m.GetMissionInfo();
            obj.Checked = m.IsComplete();
        }
    }
}
