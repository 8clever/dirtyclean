using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionController : MonoBehaviour
{
    public Mission[] missions = {};
    public GameObject content;
    public GameObject TaskItem;
    public void Start()
    {
        var controller = FindObjectOfType<GameController>();
        if (controller) {
            missions = controller.missions.ToArray();
        }
        foreach(var m in missions) {
            var i = Instantiate(TaskItem, content.transform);
            i.GetComponentInChildren<Text>().text = $"{m.type.ToString()} {m.count}/{m.requiredCount}";
            SpriteRenderer renderer;
            m.Nip.TryGetComponent<SpriteRenderer>(out renderer);
            i.GetComponentInChildren<Image>().sprite = renderer.sprite;
        }
    }
}
