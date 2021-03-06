﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powder : Nip, INip
{
    public bool CanDrag => true;

    public static string ResourcePath = "Nip/Powder";
    public GameObject AnimationPowder;
    private List<System.Type> list = new List<System.Type>();
    public void NextStep()
    {
    }

    public void OnCollision(Collision collision)
    {
        var cell = collision.gameObject.GetComponentInParent<Cell>();
        if (cell.transform.childCount == 1) return;
        foreach(Transform t in cell.transform) {
            Destroy(t.gameObject);
            AddPoints(1);
        }
        Instantiate(AnimationPowder, cell.transform);
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var powder = cell.transform.GetComponentInChildren<Powder>();
            if (powder) {
                Destroy(this.gameObject);
                Destroy(powder.gameObject);
                Instantiate(Resources.Load<Mine>(Mine.ResourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }

        OnDropDefault(cell);
    }

    // Start is called before the first frame update
    void Awake()
    {
        DefAwake();
        AddPoints(6);
    }

    public bool CanDrop(Cell cell)
    {
        if (cell.GetComponentInChildren<Powder>()) return true;
        return CanDropDefault(cell);
    }
}
