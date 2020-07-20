using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityGuard : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/SecurityGuard";

    public void NextStep()
    {
        MoveToNip(typeof(Cat));
    }

    public void OnCollision(Collision collision)
    {
        var cat = collision.transform.GetComponentInParent<Cat>();
        if (cat != null) {
            Destroy(cat.gameObject);
        }
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var security = cell.transform.GetComponentInChildren<SecurityGuard>();
            if (security) {
                Destroy(this.gameObject);
                Destroy(security.gameObject);
                Instantiate(Resources.Load<Police>(Police.resourcePath), cell.transform);
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
