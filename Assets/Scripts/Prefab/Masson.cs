using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Masson : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/Masson";

    private List<System.Type> list = new List<System.Type>();

    public void NextStep()
    {
        MoveToNips(list);
    }

    public void OnCollision(Collision collision)
    {
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
        list.Add(typeof(Chinese));
        list.Add(typeof(SecurityGuard));
        list.Add(typeof(Police));
        list.Add(typeof(Military));
    }
}
