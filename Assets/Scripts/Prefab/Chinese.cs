﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chinese : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/Chinese";

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
                Instantiate(Resources.Load<Powder>(Powder.resourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        list.Add(typeof(Rat));
        list.Add(typeof(Cat));
        list.Add(typeof(StreetDog));
        list.Add(typeof(HomeDog));
    }
}
