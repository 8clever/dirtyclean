using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityGuard : Nip, INip
{
    public bool CanDrag => true;

    public static string ResourcePath = "Nip/SecurityGuard";

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
            var security = cell.transform.GetComponentInChildren<SecurityGuard>();
            if (security) {
                Destroy(this.gameObject);
                Destroy(security.gameObject);
                Instantiate(Resources.Load<Police>(Police.ResourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }
        OnDropDefault(cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        list.Add(typeof(Cat));
        list.Add(typeof(Alien));

        AddPoints(3);
    }

    public bool CanDrop(Cell cell)
    {
        if (cell.GetComponentInChildren<SecurityGuard>()) return true;
        return CanDropDefault(cell);
    }
}
