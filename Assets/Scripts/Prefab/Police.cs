using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Police : Nip, INip
{
    public bool CanDrag => true;

    public static string ResourcePath = "Nip/Police";

    public void NextStep()
    {
        MoveToNip(typeof(Dvornik));
    }

    public void OnCollision(Collision collision)
    {
        var dvornik = collision.transform.GetComponentInParent<Dvornik>();
        if (dvornik != null) {
            Destroy(dvornik.gameObject);
            var bomj = Instantiate(Resources.Load<Bomj>(Bomj.ResourcePath), dvornik.transform.parent);
            bomj.RandomMove();
        }
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var police = cell.transform.GetComponentInChildren<Police>();
            if (police) {
                Destroy(this.gameObject);
                Destroy(police.gameObject);
                Instantiate(Resources.Load<Hunter>(Hunter.ResourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }
        OnDropDefault(cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        AddPoints(4);
    }

    public bool CanDrop(Cell cell)
    {
        if (cell.GetComponentInChildren<Police>()) return true;
        return CanDropDefault(cell);
    }
}
