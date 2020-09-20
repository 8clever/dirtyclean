using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Nip, INip
{
    public bool CanDrag => true;

    public static string ResourcePath = "Nip/Mine";

    private List<System.Type> list = new List<System.Type>();

    public void NextStep()
    {
    }

    public void OnCollision(Collision collision)
    {
        var cell = GetComponentInParent<Cell>();
        if (cell.transform.childCount == 1) return;
        
        var explosiveRange = GetComponentInChildren<ExpliseveMineRange>();
        var collider = explosiveRange?.GetComponent<BoxCollider>();
        var nipColliders = GameObject.FindObjectsOfType<Collider>();

        void DestroyNip (Nip nip) {
            // not allow remove blockers
            if (nip.GetType() == typeof(Blocker)) return;

            AddPoints(1);
            Destroy(nip.gameObject);
        }

        foreach (var c in nipColliders) {
            var sphere = c.GetComponent<SphereCollider>();
            var intersected = sphere?.bounds.Intersects(collider.bounds) ?? false;
            if (intersected) {
                var nip = c.GetComponentInParent<Nip>();
                DestroyNip(nip);
            }
        }
        
        Destroy(gameObject);
    }

    public void OnDrop(GameObject cell)
    {
        OnDropDefault(cell);
    }

    // Start is called before the first frame update
    void Awake()
    {
        DefAwake();
        AddPoints(7);
    }

    public bool CanDrop(Cell cell)
    {
        return CanDropDefault(cell);
    }
}
