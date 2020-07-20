using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kust : Nip, INip
{
    // Start is called before the first frame update
    public static string resourcePath = "Nip/Kust";

    public bool CanDrag => true;

    public void NextStep()
    {
    }

    public void OnCollision(Collision collision)
    {
    }

    public void OnDrop(GameObject cell)
    {
        OnDropDefault(cell);
    }

    void Start()
    {
        
    }

}
