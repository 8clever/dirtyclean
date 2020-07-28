using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/Mine";

    private List<System.Type> list = new List<System.Type>();

    public void NextStep()
    {
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

        AddPoints(7);
    }

    public bool CanDrop(Cell cell)
    {
        return CanDropDefault(cell);
    }
}
