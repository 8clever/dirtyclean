using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class President : Nip, INip
{
    public bool CanDrag => true;

    public static string ResourcePath = "Nip/President";
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
            var president = cell.GetComponentInChildren<President>();
            if (president) {
                Destroy(president.gameObject);
                Destroy(gameObject);
                Instantiate(Resources.Load<Masson>(Masson.ResourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }
        OnDropDefault(cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        AddPoints(5);
    }

    public bool CanDrop(Cell cell)
    {
        if (cell.GetComponentInChildren<President>()) return true;
        return CanDropDefault(cell);
    }
}
