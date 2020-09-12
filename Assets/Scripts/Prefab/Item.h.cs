using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem {
    void OnDrop (GameObject cell);

    bool CanDrop (Cell cell);
}

public class Item : MonoBehaviour
{
    public GameController controller;
    private Config config;

    private void Awake() {
        config = Config.GetConfig();
        controller = GameObject.FindObjectOfType<GameController>();
    }

    public void MoveToItemField () {
        var field = GameObject.Find("ItemField");
        if (field == null) throw new System.Exception("ItemField required");
        var pos = new Vector3(field.transform.position.x, field.transform.position.y, config.layers.items);
        this.transform.position = pos;
        this.GetComponentInChildren<Collider>().transform.position = pos;
    }

    public void NextStep () {
        controller.NextStep();
    }
}
