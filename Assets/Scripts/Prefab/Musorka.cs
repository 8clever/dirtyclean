using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musorka : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/Musorka";

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
                Instantiate(Resources.Load<Svalka>(Svalka.resourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }
        OnDropDefault(cell);
    }

    void Start()
    {
        
    }
}
