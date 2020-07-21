﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

                if (item != null) (item as IItem).OnDrop(cell.gameObject);
                if (nip != null) (nip as INip).OnDrop(cell.gameObject);
                break;
            }

            if (item != null) item.MoveToItemField();
            if (nip != null) nip.MoveToBack();
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
