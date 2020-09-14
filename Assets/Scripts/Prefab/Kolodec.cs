using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kolodec : Nip, INip
{
    public bool CanDrag => false;

    public static string ResourcePath = "Nip/Kolodec";

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

    public bool CanDrop(Cell cell)
    {
        return false;
    }
}
