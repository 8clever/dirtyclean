using UnityEngine;

public class Bomj : Nip, INip
{
    public bool CanDrag => true;

    public static string resourcePath = "Nip/Bomj";

    public void NextStep()
    {   
        RandomMove();
    }

    public void OnCollision(Collision collision)
    {
        
    }

    public void OnDrop(GameObject cell)
    {
        OnDropDefault(cell);
    }

    // Start is called before the first frame update
    void Start()
    {
        AddPoints(-10);
    }

    public bool CanDrop(Cell cell)
    {
        return CanDropDefault(cell);
    }
}
