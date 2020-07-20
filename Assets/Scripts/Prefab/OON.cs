using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OON : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/OON";

    public void NextStep()
    {
        MoveToNip(typeof(Military));
    }

    public void OnCollision(Collision collision)
    {
        var military = collision.transform.GetComponentInParent<Military>();
        if (military != null) {
            Instantiate(Resources.Load<Police>(Police.resourcePath), military.transform.parent);
            Destroy(military.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var oon = cell.GetComponentInChildren<OON>();
            if (oon) {
                Destroy(oon.gameObject);
                Destroy(this.gameObject);
                Instantiate(Resources.Load<President>(President.resourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }
        OnDropDefault(cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
