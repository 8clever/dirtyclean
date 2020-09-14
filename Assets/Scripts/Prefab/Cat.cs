using UnityEngine;
using System.Collections.Generic;

public class Cat : Nip, INip
{
    public bool CanDrag => true;

    public static string ResourcePath = "Nip/Cat";

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
        var cat = cell.transform.GetComponentInChildren<Cat>();
        if (cat) {
            Destroy(this.gameObject);
            Destroy(cat.gameObject);
            Instantiate(Resources.Load<Chinese>(Chinese.ResourcePath), cell.transform);
            GameNextStep();
            return;
        }

        OnDropDefault(cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        list.Add(typeof(Rat));
        list.Add(typeof(Alien));

        AddPoints(4);
    }

    public bool CanDrop(Cell cell)
    {
        if (cell.GetComponentInChildren<Cat>()) return true;
        return CanDropDefault(cell);
    }
}
