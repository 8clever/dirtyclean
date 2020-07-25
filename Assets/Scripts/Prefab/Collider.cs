using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collider : MonoBehaviour
{
    public bool draggable = false;

    private bool dragged = false;

    private bool isNip;

    private GameObject handPicked;

    void Start()
    {
        var nip = this.transform.parent.GetComponent<INip>();
        isNip = nip != null;
    }

    // Update is called once per frame
    void Update()
    {
        if (dragged) {
            var camera = Camera.main;
            if (!camera) throw new System.Exception("Tag MainCamera not setted");

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            this.transform.parent.position = new Vector3(ray.origin.x, ray.origin.y, Config.Layers.dragged);
            this.transform.position = new Vector3(ray.origin.x, ray.origin.y, Config.Layers.dragged);
            var hits = Physics.RaycastAll(ray);
            foreach (var hit in hits) {
                var cell = hit.collider.GetComponent<Cell>();
                if (cell) {
                    var nip = this.GetComponentInParent<INip>();
                    var item = this.GetComponentInParent<IItem>();
                    var img = cell.GetComponent<Image>();
                    var canDrop = (nip != null && nip.CanDrop(cell)) || (item != null && item.CanDrop(cell));
                    img.color = canDrop ? cell.canDrop : cell.cantDrop;
                }
            }
        }

        if (isNip) {
            if (draggable && handPicked == null) {
                handPicked = Instantiate(Resources.Load("Item/HandPicked"), this.transform.parent) as GameObject;
            }

            if (!draggable && handPicked != null) {
                Destroy(handPicked);
                handPicked = null;
            }
        }
    }

    void OnMouseDown() {
        if (!draggable) return;
        dragged = true;
    }

    void OnMouseUp() {
        if (!draggable) return;

        dragged = false;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray);
        if (hits.Length == 0) return;

        foreach (var hit in hits) {
            var cell = hit.collider.GetComponent<Cell>();
            var item = this.transform.parent.GetComponent<Item>();
            var nip = this.transform.parent.GetComponent<Nip>();

            void MoveBack () {
                if (item != null) item.MoveToItemField();
                if (nip != null) nip.MoveToBack();
            }

            if (cell) {
                if (this.transform.parent.transform.parent == cell.transform) {
                    if (isNip && draggable) {
                        draggable = false;
                        var itemField = GameObject.Find("ItemField") as GameObject;
                        if (itemField.transform.childCount == 0) {
                            Instantiate(Resources.Load<Hand>(Hand.resourcePath), itemField.transform);
                        }
                    }
                    return;
                }
                
                if (isNip && cell.isPrison && cell.transform.childCount > 0) {
                    MoveBack();
                    break;
                }

                if (item != null) (item as IItem).OnDrop(cell.gameObject);
                if (nip != null) (nip as INip).OnDrop(cell.gameObject);
                break;
            }

            MoveBack();
        }
    }

    void OnCollisionEnter(Collision other) {
        var collider = other.gameObject.GetComponent<Collider>();
        if (collider && collider.dragged) return;
        if (isNip && draggable) return;

        var nip = this.transform.parent.GetComponent<INip>();
        if (nip != null) {
            nip.OnCollision(other);
        }
    }
}
