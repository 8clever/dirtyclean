using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : Nip, INip
{

    public static string resourcesPath = "Nip/Alien";
    public bool CanDrag => true;

    private List<System.Type> list = new List<System.Type>();

    public bool CanDrop(Cell cell)
    {
        return CanDropDefault(cell);
    }

    public void NextStep()
    {
        MoveToNips(list);
    }

    public void OnCollision(Collision collision)
    {
        foreach(var type in list) {
            var nip = collision.gameObject.GetComponentInParent(type);
            if (nip) {
                AddPoints(1);
                Destroy(nip.gameObject);
            }
        }
    }

    public void OnDrop(GameObject cell)
    {
        OnDropDefault(cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        list.Add(typeof(Bomj));
        list.Add(typeof(Dvornik));
        list.Add(typeof(GreenPeace));
        list.Add(typeof(OON));
        list.Add(typeof(President));
        list.Add(typeof(Masson));
        list.Add(typeof(Chinese));
        list.Add(typeof(Police));
        list.Add(typeof(Hunter));
        list.Add(typeof(Military));

        AddPoints(7);
    }
}
