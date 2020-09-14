using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValunItem : Item, IItem
{
    public static string ResourcePath = "Item/Valun";
    public bool CanDrop(Cell cell)
    {
        if (cell.isPrison) return false;
        if (cell.transform.childCount == 0) return true;
        return false; 
    }

    public void OnDrop(GameObject cell)
    {
        if (CanDrop(cell.GetComponent<Cell>())) {
            Destroy(gameObject);
            Instantiate(Resources.Load<Valun>(Valun.ResourcePath), cell.transform);
            NextStep();
            return;
        }
        MoveToItemField();
    }
}
