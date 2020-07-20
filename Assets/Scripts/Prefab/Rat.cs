using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/Rat";

    public void NextStep()
    {
        RandomMove();
    }

    public void OnCollision(Collision collision)
    {
        
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var rat = cell.transform.GetComponentInChildren<Rat>();
            if (rat) {
                Destroy(this.gameObject);
                Destroy(rat.gameObject);
                Instantiate(Resources.Load<Cat>(Cat.resourcePath), cell.transform);
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
