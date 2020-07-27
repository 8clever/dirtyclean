using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetDog : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/StreetDog";

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
        OnDropDefault(cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        AddPoints(1);
    }

    public bool CanDrop(Cell cell)
    {
        return CanDropDefault(cell);
    }
}
