using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Nip, INip
{
    public bool CanDrag => true;

    public static string ResourcePath = "Nip/Tank";

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

    public bool CanDrop(Cell cell)
    {
        return CanDropDefault(cell);
    }

    // Start is called before the first frame update
    void Awake()
    {
        DefAwake();
        
        list.Add(typeof(Bomj));
        list.Add(typeof(Dvornik));
        list.Add(typeof(GreenPeace));
        list.Add(typeof(OON));
        list.Add(typeof(President));
        list.Add(typeof(Masson));
        list.Add(typeof(Chinese));
        list.Add(typeof(SecurityGuard));
        list.Add(typeof(Police));
        list.Add(typeof(Military));
        list.Add(typeof(Musorka));
        list.Add(typeof(Svalka));
        list.Add(typeof(Rat));
        list.Add(typeof(Cat));
        list.Add(typeof(StreetDog));
        list.Add(typeof(HomeDog));
        list.Add(typeof(Alien));

        AddPoints(7);
    }
}
