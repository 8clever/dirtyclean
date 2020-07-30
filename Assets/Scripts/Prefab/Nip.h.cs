using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INip
{
    bool CanDrag { get; }
    void NextStep ();

    void OnCollision (Collision collision);

    void OnDrop (GameObject cell);

    bool CanDrop (Cell cell);
}

public class Nip : MonoBehaviour
{

    public Transform prevParent;

    public Nip GetClosestNip (System.Type type) {
        var objects = GameObject.FindObjectsOfType(type) as Nip[];
        if (objects == null) return null;
        if (objects.Length == 0) return null; 

        var destination = objects[0];
        foreach (var obj in objects) {
            var currentDist = Vector3.Distance(this.transform.parent.position, destination.transform.position);
            var newDist = Vector3.Distance(this.transform.parent.position, obj.transform.parent.position);
            if (newDist < currentDist) {
                destination = obj;
            }
        }
        return destination;
    }

    public void MoveToNips (List<System.Type> list) {
        if (this == null) return;

        var nips = new List<Nip>();
        list.ForEach(l => {
            var n = GetClosestNip(l);
            if (n != null) {
                nips.Add(n);
            }
        });
        nips.Sort((a, b) => {
            var distA = Vector3.Distance(this.transform.parent.position, a.transform.parent.position);
            var distB = Vector3.Distance(this.transform.parent.position, b.transform.parent.position);
            return distA.CompareTo(distB);
        });
        if (nips.Count > 0) {
            MoveToNip(nips[0].GetType());
        } else {
            RandomMove();
        }
    }

    public void MoveToNip (System.Type type) {
        if (this == null) return;

        var destination = GetClosestNip(type);
        if (destination == null) {
            RandomMove();
            return;
        };
        
        var startPoint = transform.parent.position;
        var direction =  destination.transform.parent.position - transform.parent.position;
        var ray = new Ray(startPoint, direction);
        MoveByRay(ray, type);
    }

    public void MoveByRay (Ray destination, System.Type type) {
        var cell = transform.parent.GetComponent<Cell>();
        if (cell.isPrison) return;

        var pos = transform.parent.position;
        var dist = 100;
        List<Ray> rays = new List<Ray>();
        rays.Add(new Ray(pos, new Vector3(pos.x, pos.y + dist, pos.z) - pos));
        rays.Add(new Ray(pos, new Vector3(pos.x - dist, pos.y, pos.z) - pos));
        rays.Add(new Ray(pos, new Vector3(pos.x + dist, pos.y, pos.z) - pos));
        rays.Add(new Ray(pos, new Vector3(pos.x, pos.y - dist, pos.z) - pos));
        rays.Sort((ray1, ray2) => {
            var distance1 = Vector3.Distance(destination.direction, ray1.direction);
            var distance2 = Vector3.Distance(destination.direction, ray2.direction);
            return distance1.CompareTo(distance2);
        });
        foreach (var ray in rays) {
            var hits = GetCellHits(ray);
            if (hits.Length > 0) {
                var hit = GetClosestHit(hits);
                var isValid = IsValidHit(hit, type);
                if (isValid) {
                    prevParent = transform.parent;
                    transform.SetParent(hit.collider.transform);
                    return;
                };
            }
        }
    }

    public void RandomMove () {
        var x = Random.Range(-1, 2) * 100;
        var y = Random.Range(-1, 2) * 100;
        if (this == null) return;

        var destination = transform.parent.position + new Vector3(x, x == 0 ? y : 0, 0);
        var ray = new Ray(transform.parent.position, destination - transform.parent.position);
        MoveByRay(ray, typeof(Cell));
    }

    private RaycastHit[] GetCellHits (Ray ray) {
        var hits = Physics.RaycastAll(ray, Mathf.Infinity, 1 << 8);
        return hits;
    }

    private bool IsValidHit (RaycastHit hit, System.Type type) {
        var cell = hit.collider.GetComponent<Cell>();
        if (cell && cell.isPrison) {
            return false;
        }

        if (hit.collider.transform.childCount > 1) return false;

        for (int n = 0; n < hit.collider.transform.childCount; n++) {
            var child = hit.collider.transform.GetChild(n);
            var requiredNip = child.GetComponent(type);
            if (requiredNip) {
                return true;
            }
        }

        if (hit.collider.transform.childCount > 0) {
            return false;
        }

        return true;
    }

    private RaycastHit GetClosestHit (RaycastHit[] hits) {
        RaycastHit hit = hits[0];
        foreach (var h in hits) {
            if (h.distance < hit.distance) {
                hit = h;
            } 
        }
        return hit;
    }

    public void Update () {
        UpdateDefault();
    }

    public void UpdateDefault () {
        var parentPos = new Vector3(this.transform.parent.position.x, this.transform.parent.position.y, Config.Layers.nips);
        if (parentPos != this.transform.position) {
            var nextPosition = Vector3.MoveTowards(this.transform.position, parentPos, Config.Nip.moveSpeed);
            this.transform.position = nextPosition;
        }
    }

    public void SetDraggable (bool value) {
        this.transform.GetComponentInChildren<Collider>().draggable = value;
    }

    public void GameNextStep () {
        var controller = GameObject.FindObjectOfType<GameController>();
        controller.NextStep();
    }

    public void SetParentAndNext (GameObject cell) {
        this.transform.SetParent(cell.transform);
        this.SetDraggable(false);
        this.GameNextStep();
    }

    public void OnDropDefault (GameObject cell) {
        if (cell.transform.childCount == 0) {
            var c = cell.GetComponent<Cell>();
            if (c.isPrison) {
                AddPoints(1);
            }
            SetParentAndNext(cell);
            return;
        }
        MoveToBack();
    }

    public bool CanDropDefault (Cell cell) {
        if (cell.transform.childCount == 0) return true;
        return false;
    }

    public void MoveToBack () {
        this.transform.position = new Vector3(this.transform.parent.position.x, this.transform.parent.position.y, Config.Layers.nips);
    }

    public void MoveToPrevParent () {
        var parent = this.transform.parent;
        this.transform.SetParent(prevParent);
        prevParent = parent;
    }

    public void AddPoints (int number) {
        var controller = GameObject.FindObjectOfType<GameController>();
        controller.AddPointsToPoints(number);
    }

    public void OnCollisionList (Collision collision, List<System.Type> list) {
        foreach(var type in list) {
            var nip = collision.gameObject.GetComponentInParent(type);
            if (nip) {
                AddPoints(1);
                Destroy(nip.gameObject);
            }
        }
    }
}
