using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetDog : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/StreetDog";

    private List<System.Type> list = new List<System.Type>();

    public void NextStep()
    {
        MoveToNips(list);
    }

    public void OnCollision(Collision collision)
    {
        OnCollisionList(collision, list);
    }

    public void OnDrop(GameObject cell)
    {
        OnDropDefault(cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        list.Add(typeof(Alien));
        list.Add(typeof(Cat));

        AddPoints(1);
    }

    public bool CanDrop(Cell cell)
    {
        return CanDropDefault(cell);
    }
}
