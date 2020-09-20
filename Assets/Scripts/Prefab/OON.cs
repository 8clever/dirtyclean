using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OON : Nip, INip
{
    public bool CanDrag => true;

    public static string ResourcePath = "Nip/OON";

    public void NextStep()
    {
        MoveToNip(typeof(Military));
    }

    public void OnCollision(Collision collision)
    {
        var military = collision.transform.GetComponentInParent<Military>();
        if (military != null) {
            AddPoints(1);
            Destroy(military.gameObject);
            var police = Instantiate(Resources.Load<Police>(Police.ResourcePath), military.transform.parent);
            police.RandomMove();
        }
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var oon = cell.GetComponentInChildren<OON>();
            if (oon) {
                Destroy(oon.gameObject);
                Destroy(this.gameObject);
                Instantiate(Resources.Load<President>(President.ResourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }
        OnDropDefault(cell);
    }

    // Start is called before the first frame update
    void Awake()
    {
        DefAwake();
        AddPoints(4);
    }

    public bool CanDrop(Cell cell)
    {
        if (cell.GetComponentInChildren<OON>()) return true;
        return CanDropDefault(cell);
    }
}
