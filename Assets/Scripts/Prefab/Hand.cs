using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : Item, IItem
{

    public static string resourcePath = "Item/Hand";
    public void OnDrop(GameObject cell)
    {   
        if (cell.transform.childCount == 0) {
            Destroy(this.gameObject);
            var controller = GameObject.FindObjectOfType<GameController>();
            controller.AddPointsToHealth(1);
            NextStep();
            return;
        }

        if (cell.transform.childCount > 0) {
            var nip = cell.GetComponentInChildren<INip>();
            if (nip != null && nip.CanDrag) {
                Destroy(this.gameObject);
                var obj = cell.transform.GetChild(0);
                var collider = obj.GetComponentInChildren<Collider>();
                collider.draggable = true;
                return;
            }
        }

        MoveToItemField();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
