using System;
using UnityEngine;

public class Dvornik : Nip, INip
{
    public bool CanDrag => true;

    public static string ResourcePath = "Nip/Dvornik";

    public void NextStep ()
    {   
        MoveToNip(typeof(Musorka));
    }

    // Start is called before the first frame update
    private void Awake()
    {
        DefAwake();
        AddPoints(1);
    }

    public void OnCollision(Collision collision)
    {
        var musorka = collision.gameObject.GetComponentInParent<Musorka>();
        if (musorka == null) return;

        AddPoints(1);
        Destroy(musorka.gameObject);
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var dvornik = cell.GetComponentInChildren<Dvornik>();
            if (dvornik) {
                Destroy(dvornik.gameObject);
                Destroy(this.gameObject);
                Instantiate(Resources.Load<Buldozer>(Buldozer.ResourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }
        
        OnDropDefault(cell);
    }

    public bool CanDrop(Cell cell)
    {
        if (cell.GetComponentInChildren<Dvornik>()) return true;
        return CanDropDefault(cell);
    }
}
