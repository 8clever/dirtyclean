using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Military : Nip, INip
{
    public bool CanDrag => true;

    public static string ResourcePath = "Nip/Military";

    public void NextStep()
    {
        MoveToNip(typeof(President));
    }

    public void OnCollision(Collision collision)
    {
        var president = collision.gameObject.GetComponentInParent<President>();
        if (president) {
            AddPoints(1);
            Destroy(president.gameObject);
            var bomj = Instantiate(Resources.Load<Bomj>(Bomj.ResourcePath), transform.parent);
            bomj.MoveToClosestEmptyCell();
        }
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var military = cell.transform.GetComponentInChildren<Military>();
            if (military) {
                Destroy(gameObject);
                Destroy(military.gameObject);
                Instantiate(Resources.Load<Tank>(Tank.ResourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }

        OnDropDefault(cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        AddPoints(6);
    }

    public bool CanDrop(Cell cell)
    {
        if (cell.GetComponentInChildren<Military>()) return true;
        return CanDropDefault(cell);
    }
}
