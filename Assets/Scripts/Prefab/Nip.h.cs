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
    public AudioClip awakeAudio;
    public AudioClip destroyAudio;
    public int Grade = 0;
    private Config config;
    private Transform prevParent;
    private GameController controller;
    public static readonly string TAG = "nip";
    private void Awake () {
        DefAwake();
    }
    public void DefAwake () {
        config = Config.GetConfig();
        controller = GameObject.FindObjectOfType<GameController>();
        if (controller.gameInitialized) {
            controller.SetMission(this, Mission.Type.Create, 1);
            controller.SetMission(this, Mission.Type.Collect, 0);
        }
        if (controller.gameInitialized && awakeAudio) {
            MusicController.PlayOnce(awakeAudio);
        }
    }

    private void OnDestroy() {
        if (controller.gameInitialized) {
            controller.SetMission(this, Mission.Type.Destroy, 1);    
            controller.SetMission(this, Mission.Type.Collect, 0);
        }
        if (destroyAudio) {
            MusicController.PlayOnce(destroyAudio);
        }
    }

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

    public Nip GetClosestNip (Nip nip) {
        var nips = new List<Nip>(GameObject.FindObjectsOfType<Nip>()).FindAll(obj => {
            return nip.GetName() == obj.GetName();
        });
        if (nips.Count == 0) return null;

        var destination = nips[0];
        foreach (var obj in nips) {
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
            var distA = Vector3.Distance(transform.parent.position, a.transform.parent.position);
            var distB = Vector3.Distance(transform.parent.position, b.transform.parent.position);
            return distA.CompareTo(distB);
        });
        if (nips.Count > 0) {
            MoveToNip(nips[0].GetType());
        } else {
            RandomMove();
        }
    }

    public void MoveToNips (List<Nip> list) {
        if (this == null) return;

        var nips = new List<Nip>();
        list.ForEach(l => {
            var n = GetClosestNip(l);
            if (!n) return;
            nips.Add(n);
        });
        nips.Sort((a, b) => {
            var distA = Vector3.Distance(transform.parent.position, a.transform.parent.position);
            var distB = Vector3.Distance(transform.parent.position, b.transform.parent.position);
            return distA.CompareTo(distB);
        });
        if (nips.Count > 0) {
            MoveToNip(nips[0]);
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

    public void MoveToNip (Nip nip) {
        if (this == null) return;

        var destination = GetClosestNip(nip);
        if (destination == null) {
            RandomMove();
            return;
        };
        
        var startPoint = transform.parent.position;
        var direction =  destination.transform.parent.position - transform.parent.position;
        var ray = new Ray(startPoint, direction);
        MoveByRay(ray, nip);
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

    public void MoveByRay (Ray destination, Nip nip) {
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
                var isValid = IsValidHit(hit, nip);
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
        MoveByRay(ray, null as Nip);
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

        if (cell.transform.childCount > 0) {
            var powder = cell.GetComponentInChildren<Powder>();
            var mine = cell.GetComponentInChildren<Mine>();
            var requriedNip = type == null ? null : cell.GetComponentInChildren(type);
            if (requriedNip && cell.transform.childCount == 1) {}
            // allow move for specific nips
            else if (powder) {}
            else if (mine) {}
            // no allow move
            else return false;
        };

        return true;
    }

    private bool IsValidHit (RaycastHit hit, Nip nip) {
        var cell = hit.collider.GetComponent<Cell>();
        if (cell && cell.isPrison) {
            return false;
        }

        if (cell.transform.childCount > 0) {
            var powder = cell.GetComponentInChildren<Powder>();
            var mine = cell.GetComponentInChildren<Mine>();
            var requriedNip = (
                nip == null ? 
                false :
                nip.GetName() == cell.GetComponentInChildren<Nip>()?.GetName()
            );
            if (requriedNip && cell.transform.childCount == 1) {}
            // allow move for specific nips
            else if (powder) {}
            else if (mine) {}
            // no allow move
            else return false;
        };

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
        var parentPos = new Vector3(transform.parent.position.x, transform.parent.position.y, config.layers.nips);
        if (parentPos != transform.position) {
            var nextPosition = Vector3.MoveTowards(transform.position, parentPos, config.nip.moveSpeed);
            transform.position = nextPosition;
        }
    }

    public void SetDraggable (bool value) {
        this.transform.GetComponentInChildren<Collider>().draggable = value;
    }

    public void GameNextStep () {
        controller.NextStep();
    }

    public void SetParentAndNext (GameObject cell) {
        this.transform.SetParent(cell.transform);
        this.SetDraggable(false);
        this.GameNextStep();
    }

    public void OnDropDefault (GameObject cell) {
        if (cell.transform.childCount == 0) {
            if (awakeAudio) {
                MusicController.PlayOnce(awakeAudio);
            }
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
        transform.position = new Vector3(this.transform.parent.position.x, this.transform.parent.position.y, config.layers.nips);
    }

    public void MoveToClosestEmptyCell () {
        var gameObjects = GameObject.FindGameObjectsWithTag("cell");
        var emptyCells = new List<Cell>();
        foreach(var g in gameObjects) {
            var cell = g.GetComponent<Cell>();
            if (cell?.transform.childCount == 0 && cell?.isPrison == false) {
                emptyCells.Add(cell);
            }
        }
        if (emptyCells.Count == 0) return;

        emptyCells.Sort((cell1, cell2) => {
            var distance1 = Vector3.Distance(cell1.transform.position, transform.position);
            var distance2 = Vector3.Distance(cell2.transform.position, transform.position);
            return distance1.CompareTo(distance2);
        });
        transform.SetParent(emptyCells[0].transform);
    }

    public void AddPoints (int number) {
        if (!controller.gameInitialized) return;
        if (number == 0) return;
        
        controller.AddPointsToPoints(number);
        var anim = Resources.Load<GameObject>("Animations/PointAddAnimation");
        var obj = Instantiate(anim, transform);
        var symbol = number > 0 ? "+" : "";
        obj.GetComponentInChildren<TMPro.TextMeshPro>().text = $"{symbol}{number}";
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

    [System.Serializable]
    public class Save {
        public string cellName;
        public string resourcePath;

        public void Restore () {
            var cell = GameObject.Find(cellName);
            Instantiate(Resources.Load(resourcePath), cell.transform);
        }
    }

    public string GetName () {
        return name.Replace("(Clone)", "");
    }

    public Save GetSave () {
        return new Save()
        {
            cellName = transform.parent.name,
            resourcePath = $"Nip/{GetName()}"
        };
    }
}
