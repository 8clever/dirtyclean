using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/Cat";

    public void NextStep()
    {
        MoveToNip(typeof(Rat));
    }

    public void OnCollision(Collision collision)
    {
        var rat = collision.transform.GetComponentInParent<Rat>();
        if (rat != null) {
            Destroy(rat.gameObject);
        }
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var cat = cell.transform.GetComponentInChildren<Cat>();
            if (cat) {
                Destroy(this.gameObject);
                Destroy(cat.gameObject);
                Instantiate(Resources.Load<Chinese>(Chinese.resourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
