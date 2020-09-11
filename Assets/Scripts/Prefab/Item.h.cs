using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem {
    void OnDrop (GameObject cell);

    bool CanDrop (Cell cell);
}

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private Config config;

    private void Awake() {
        config = Config.GetConfig();
    }

    public void MoveToItemField () {
        var field = GameObject.Find("ItemField");
        if (field == null) throw new System.Exception("ItemField required");
        var pos = new Vector3(field.transform.position.x, field.transform.position.y, config.layers.items);
        this.transform.position = pos;
        this.GetComponentInChildren<Collider>().transform.position = pos;
    }

    public void NextStep () {
        var controller = GameObject.FindObjectOfType<GameController>();
        controller.NextStep();
    }
}
