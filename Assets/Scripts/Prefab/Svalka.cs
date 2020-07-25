using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Svalka : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/Svalka";

    public void NextStep()
    {

    }

    public void OnCollision(Collision collision)
    {

    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var svalka = cell.GetComponentInChildren<Svalka>();
            if (svalka) {
                Destroy(svalka.gameObject);
                Destroy(this.gameObject);
                Instantiate(Resources.Load<Rat>(Rat.resourcePath), cell.transform);
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

    public bool CanDrop(Cell cell)
    {
        if (cell.GetComponentInChildren<Svalka>()) return true;
        return CanDropDefault(cell);
    }
}
