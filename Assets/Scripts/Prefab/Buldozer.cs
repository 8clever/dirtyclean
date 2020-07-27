using UnityEngine;

public class Buldozer : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/Buldozer";

    public void NextStep()
    {
        MoveToNip(typeof(Svalka));
    }

    public void OnCollision(Collision collision)
    {
        var svalka = collision.transform.GetComponentInParent<Svalka>();
        if (svalka != null) {
            AddPoints(1);
            Destroy(svalka.gameObject);
        }
    }

    public void OnDrop(GameObject cell)
    {
        if (cell) {
            var buldozer = cell.GetComponentInChildren<Buldozer>();
            if (buldozer) {
                Destroy(buldozer.gameObject);
                Destroy(this.gameObject);
                Instantiate(Resources.Load<GreenPeace>(GreenPeace.resourcePath), cell.transform);
                GameNextStep();
                return;
            }
        }
        
        OnDropDefault(cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        AddPoints(2);
    }

    public bool CanDrop(Cell cell)
    {
        if (cell.GetComponentInChildren<Buldozer>()) return true;
        return CanDropDefault(cell);
    }
}
