﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Military : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/Military";

    public void NextStep()
    {
        MoveToNip(typeof(President));
    }

    public void OnCollision(Collision collision)
    {
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var military = cell.transform.GetComponentInChildren<Military>();
            if (military) {
                Destroy(this.gameObject);
                Destroy(military.gameObject);
                Instantiate(Resources.Load<Tank>(Tank.resourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }

        OnDropDefault(cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
