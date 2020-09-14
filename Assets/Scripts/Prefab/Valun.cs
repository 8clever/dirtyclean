﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valun : Nip, INip
{
    public bool CanDrag => true;

    public static string ResourcePath = "Nip/Valun";
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

    public bool CanDrop (Cell cell) {
            if (cell.isPrison) return false;
        return CanDropDefault(cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
