using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Masson : Nip, INip
{
    public bool CanDrag => true;

    public static string ResourcePath = "Nip/Masson";

    private List<System.Type> list = new List<System.Type>();

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
        var masson = cell.GetComponentInChildren<Masson>();
        if (masson) {
            Destroy(this.gameObject);
            Destroy(masson.gameObject);
            Instantiate(Resources.Load<Alien>(Alien.resourcesPath), cell.transform);
            GameNextStep();
            return;
        }

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
        list.Add(typeof(Chinese));
        list.Add(typeof(SecurityGuard));
        list.Add(typeof(Police));
        list.Add(typeof(Military));

        AddPoints(6);
    }
}
