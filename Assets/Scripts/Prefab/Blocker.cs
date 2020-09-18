using UnityEngine;
using UnityEngine.UI;
// Required only for block some cells;
public class Blocker : Nip, INip
{
    public bool CanDrag => false;

    public static readonly string ResourcePath = "Nip/Blocker";

    private Image image;

    public bool CanDrop(Cell cell)
    {
        return false;
    }

    public void NextStep()
    {
    }

    public void OnCollision(Collision collision)
    {
    }

    public void OnDrop(GameObject cell)
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        image = transform.parent.GetComponent<Image>();
    }

    new void Update () {
        UpdateDefault();
    }
}
