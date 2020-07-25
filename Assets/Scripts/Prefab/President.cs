using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class President : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/President";
    public void NextStep()
    {
    }

    public void OnCollision(Collision collision)
    {
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var president = cell.GetComponentInChildren<President>();
            if (president) {
                Destroy(president.gameObject);
                Destroy(this.gameObject);
                Instantiate(Resources.Load<Masson>(Masson.resourcePath), cell.transform);
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
        if (cell.GetComponentInChildren<President>()) return true;
        return CanDropDefault(cell);
    }
}
