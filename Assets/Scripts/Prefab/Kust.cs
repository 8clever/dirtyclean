using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kust : Nip, INip
{
    // Start is called before the first frame update
    public static string ResourcePath = "Nip/Kust";

    public bool CanDrag => true;

    public void NextStep()
    {
    }

    public void OnCollision(Collision collision)
    {
    }

    public void OnDrop(GameObject cell)
    {
        var c = cell.GetComponent<Cell>();
        if (c.isPrison) {
            MoveToBack();
            return;
        }
        OnDropDefault(cell);
    }

    void Start()
    {
        
    }

    public bool CanDrop(Cell cell)
    {
        if (cell.isPrison) return false;
        return CanDropDefault(cell);
    }
}
