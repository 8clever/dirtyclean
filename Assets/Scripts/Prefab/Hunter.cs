using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Nip, INip
{
    public bool CanDrag => true;

    public static string ResourcePath = "Nip/Hunter";

    private List<System.Type> list = new List<System.Type>();

    public void NextStep()
    {
        MoveToNips(list);
    }

    public void OnCollision(Collision collision)
    {
        foreach (var nipType in list) {
            var nip = collision.transform.GetComponentInParent(nipType);
            if (nip != null) {
                AddPoints(1);
                Destroy(nip.gameObject);
            }
        }
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var hunter = cell.transform.GetComponentInChildren<Hunter>();
            if (hunter) {
                Destroy(this.gameObject);
                Destroy(hunter.gameObject);
                Instantiate(Resources.Load<Military>(Military.ResourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }

        OnDropDefault(cell);
    }

    void Awake()
    {
        DefAwake();
        
        list.Add(typeof(Cat));
        list.Add(typeof(StreetDog));
        list.Add(typeof(HomeDog));
        list.Add(typeof(Rat));
    
        AddPoints(5);
    }

    public bool CanDrop(Cell cell)
    {
        if (cell.GetComponentInChildren<Hunter>()) return true;
        return CanDropDefault(cell);
    }
}
