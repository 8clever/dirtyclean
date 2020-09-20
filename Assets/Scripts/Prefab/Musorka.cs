using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musorka : Nip, INip
{
    public bool CanDrag => true;

    public static string ResourcePath = "Nip/Musorka";

    public void NextStep () {

    }

    public void OnCollision(Collision collision)
    {
        
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var musorka = cell.transform.GetComponentInChildren<Musorka>();
            if (musorka) {
                Destroy(this.gameObject);
                Destroy(musorka.gameObject);
                Instantiate(Resources.Load<Svalka>(Svalka.ResourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }
        OnDropDefault(cell);
    }

    void Awake()
    {
        DefAwake();
        AddPoints(1);
    }

    public bool CanDrop(Cell cell)
    {
        if (cell.GetComponentInChildren<Musorka>()) return true;
        return CanDropDefault(cell);
    }
}
