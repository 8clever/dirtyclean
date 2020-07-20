using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/Mine";

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

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
