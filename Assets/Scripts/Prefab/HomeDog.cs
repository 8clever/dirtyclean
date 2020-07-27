using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeDog : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/HomeDog";

    public void NextStep()
    {
        MoveToNip(typeof(Cat));
    }

    public void OnCollision(Collision collision)
    {
        var cat = collision.transform.GetComponentInParent<Cat>();
        if (cat != null) {
            AddPoints(1);
            Destroy(cat.gameObject);
        }
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var homeDog = cell.transform.GetComponentInChildren<HomeDog>();
            if (homeDog) {
                Destroy(this.gameObject);
                Destroy(homeDog.gameObject);
                Instantiate(Resources.Load<SecurityGuard>(SecurityGuard.resourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }

        OnDropDefault(cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        AddPoints(2);
    }

    public bool CanDrop(Cell cell)
    {
        if (cell.GetComponentInChildren<HomeDog>()) return true;
        return CanDropDefault(cell);
    }
}
