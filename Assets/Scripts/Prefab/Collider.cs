using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider : MonoBehaviour
{
    public bool draggable = false;

    private bool dragged = false;

    private bool isNip;

    private GameObject handPicked;

    private bool isMouseDown = false;
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
            this.transform.parent.position = new Vector3(ray.origin.x, ray.origin.y, this.transform.parent.position.z);
            this.transform.position = new Vector3(ray.origin.x, ray.origin.y, this.transform.position.z);
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

    IEnumerator RestoreMouseDown () {
        yield return new WaitForSeconds(0.5f);
        isMouseDown = false;
    }

    void OnMouseDown() {
        if (isNip && draggable && isMouseDown) {
            draggable = false;
            isMouseDown = false;
            var itemField = GameObject.Find("ItemField") as GameObject;
            if (itemField.transform.childCount == 0) {
                Instantiate(Resources.Load<Hand>(Hand.resourcePath), itemField.transform);
            }
        }
        isMouseDown = true;
        if (!draggable) return;
        dragged = true;
    }

    void OnMouseUp() {
        StartCoroutine(RestoreMouseDown());
        if (!draggable) return;

        dragged = false;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray);
        if (hits.Length == 0) return;

        foreach (var hit in hits) {
            var cell = hit.collider.GetComponent<Cell>();

            if (cell) {
                if (this.transform.parent.transform.parent == cell.transform) {
                    return;
                }

                var nip = this.transform.parent.GetComponent<INip>();
                var item = this.transform.parent.GetComponent<IItem>();

                if (item != null) item.OnDrop(cell.gameObject);
                if (nip != null) nip.OnDrop(cell.gameObject);
            }
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
