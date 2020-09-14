using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chinese : Nip, INip
{
    public bool CanDrag => true;

    public static string ResourcePath = "Nip/Chinese";

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
            var chinese = cell.transform.GetComponentInChildren<Chinese>();
            if (chinese) {
                Destroy(this.gameObject);
                Destroy(chinese.gameObject);
                Instantiate(Resources.Load<Powder>(Powder.ResourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }

        OnDropDefault(cell);
    }

    public bool CanDrop(Cell cell)
    {
        if (cell.GetComponentInChildren<Chinese>()) return true;
        return CanDropDefault(cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        list.Add(typeof(Rat));
        list.Add(typeof(Cat));
        list.Add(typeof(StreetDog));
        list.Add(typeof(HomeDog));

        AddPoints(5);
    }
}
