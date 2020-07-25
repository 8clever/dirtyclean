using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenPeace : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/GreenPeace";

    public void NextStep()
    {
        MoveToNip(typeof(Hunter));
    }

    public void OnCollision(Collision collision)
    {
        var hunter = collision.transform.GetComponentInParent<Hunter>();
        if (hunter != null) {
            Destroy(hunter.gameObject);
        }
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var greenPeace = cell.transform.GetComponentInChildren<GreenPeace>();
            if (greenPeace) {
                Destroy(this.gameObject);
                Destroy(greenPeace.gameObject);
                Instantiate(Resources.Load<OON>(OON.resourcePath), cell.transform);
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
        if (cell.GetComponentInChildren<GreenPeace>()) return true;
        return CanDropDefault(cell);
    }
}
