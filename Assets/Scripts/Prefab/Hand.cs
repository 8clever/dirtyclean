﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : Item, IItem
{

    public static string resourcePath = "Item/Hand";
    public void OnDrop(GameObject cell)
    {   
        if (cell.transform.childCount == 0) {
            Destroy(this.gameObject);
            var controller = GameObject.FindObjectOfType<GameController>();
            controller.AddPointsToHealth(1);
            controller.AddPointsToPoints(1);
            NextStep();
            return;
        }

        if (cell.transform.childCount > 0) {
            var nip = cell.GetComponentInChildren<INip>();
            if (nip != null && nip.CanDrag) {
                Destroy(this.gameObject);
                var obj = cell.transform.GetChild(0);
                var collider = obj.GetComponentInChildren<Collider>();
                collider.draggable = true;
                return;
            }
        }

        MoveToItemField();
    }

    public bool CanDrop(Cell cell)
    {
        if (cell.transform.childCount == 0) return true;
        if (cell.transform.childCount > 0) {
            var nip = cell.GetComponentInChildren<INip>();
            if (nip != null && nip.CanDrag) {
                return true;
            }
        }
        return false;
    }
}
