using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Required only for block some cells;
public class Blocker : Nip, INip
{
    public bool CanDrag => false;

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
