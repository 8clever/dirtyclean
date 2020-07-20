using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powder : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/Powder";
    public void NextStep()
    {
    }

    public void OnCollision(Collision collision)
    {
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var powder = cell.transform.GetComponentInChildren<Powder>();
            if (powder) {
                Destroy(this.gameObject);
                Destroy(powder.gameObject);
                Instantiate(Resources.Load<Mine>(Mine.resourcePath), cell.transform);
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
