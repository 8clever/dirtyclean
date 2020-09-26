using System.Collections.Generic;
using UnityEngine;

public class ItemMain: Item
{
    public bool CanDrag = true;
    public List<Item.Drop> DropList = new List<Drop>();
    public bool AllowDropToPrison = false;
    public Nip CreateAfterDropOnEmptyCell;
    public int Price = 0;
    public void OnDrop(GameObject cell) {
        var c = cell.GetComponent<Cell>();
        if (AllowDropToPrison == false) {
            if (c.isPrison) {
                MoveToItemField();
                return;
            }
        }
        if (CreateAfterDropOnEmptyCell && cell && cell.transform.childCount == 0) {
            Destroy(gameObject);
            Instantiate(CreateAfterDropOnEmptyCell, cell.transform);
            NextStep();
            return;
        }
        foreach(var drop in DropList) {
            var nip = cell.GetComponentInChildren(drop.To.GetType());
            if (nip) {
                Destroy(gameObject);
                Destroy(nip.gameObject);
                Instantiate(drop.CreateAfterDrop, cell.transform);
                NextStep();
                return;
            }
        }
        MoveToItemField();
    }

    public bool CanDrop (Cell cell) {
        if (AllowDropToPrison == false) {
            if (cell.isPrison) return false;
        }
        if (CreateAfterDropOnEmptyCell && cell.transform.childCount == 0) {
            return true;
        }
        foreach(var drop in DropList) {
            var nip = cell.GetComponentInChildren(drop.To.GetType());
            if (nip) {
                return true;
            }
        }
        return false;
    }
}