using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Nip, INip
{
    public bool CanDrag => false;

    public static string resourcePath = "Nip/Tree";
    public void NextStep()
    {
    }

    public void OnCollision(Collision collision)
    {
    }

    public void OnDrop(GameObject cell)
    {
    }

    // Start is called before the first frame update

    void Start()
    {
        
    }
}
