using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeDog : Nip, INip
{
    public bool CanDrag => true;

    public static string ResourcePath = "Nip/HomeDog";

    private List<System.Type> list = new List<System.Type>();

    public void NextStep()
    {
        MoveToNips(list);
    }

    public void OnCollision(Collision collision)
    {
        OnCollisionList(collision, list);
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var homeDog = cell.transform.GetComponentInChildren<HomeDog>();
            if (homeDog) {
                Destroy(this.gameObject);
                Destroy(homeDog.gameObject);
                Instantiate(Resources.Load<SecurityGuard>(SecurityGuard.ResourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }

        OnDropDefault(cell);
    }

    // Start is called before the first frame update
    void Awake()
    {
        DefAwake();
        list.Add(typeof(Cat));
        list.Add(typeof(Alien));

        AddPoints(2);
    }

    public bool CanDrop(Cell cell)
    {
        if (cell.GetComponentInChildren<HomeDog>()) return true;
        return CanDropDefault(cell);
    }
}
