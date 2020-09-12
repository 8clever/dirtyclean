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

    private GameController controller;

    private Config config;

    private void Awake() {
        config = Config.GetConfig();
        controller = GameObject.FindObjectOfType<GameController>();
    }

    void Start()
    {
        var nip = transform.parent.GetComponent<INip>();
        isNip = nip != null;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isPause) return;

        if (dragged) {
            OnCellCanDrop(Hit(true));
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

        // required for items which placed directly on cell
        if (Input.GetMouseButtonDown(0) && dragged == false) {
            var mayka = transform.parent.GetComponent<Mayka>();
            var hand = transform.parent.GetComponent<Hand>();
            if (mayka) {
                OnMouseUp();
            }
            if (hand) {
                var cell = Hit();
                if (cell && cell.transform.childCount == 0) {
                    OnMouseUp();
                }
            }
        }
    }

    void OnCellDrop (Cell cell) {
        Debug.Log(cell);
        
        var item = transform.parent.GetComponent<Item>();
        var nip = transform.parent.GetComponent<Nip>();

        void MoveBackItem () {
            if (item != null) item.MoveToItemField();
        }

        void MoveBackNip () {
            if (nip != null) nip.MoveToBack();
        }

        if (!cell) {
            MoveBackItem();
            MoveBackNip();
            return;
        };

        if (transform.parent.transform.parent == cell.transform) {
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
            MoveBackNip();
        }

        if (item != null) (item as IItem).OnDrop(cell.gameObject);
        if (nip != null) (nip as INip).OnDrop(cell.gameObject);
    }

    void OnCellCanDrop (Cell cell) {
        if (!cell) return;

        var nip = GetComponentInParent<INip>();
        var item = GetComponentInParent<IItem>();
        var img = cell.GetComponent<Image>();
        var canDrop = (nip != null && nip.CanDrop(cell)) || (item != null && item.CanDrop(cell));
        img.color = canDrop ? cell.canDrop : cell.cantDrop;
    }

    Cell Hit (bool setPosition = false) {
        var camera = Camera.main;
        if (!camera) throw new System.Exception("Tag MainCamera not setted");

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (setPosition) {
            transform.parent.position = new Vector3(ray.origin.x, ray.origin.y, config.layers.dragged);
            transform.SetParent(transform.parent);
        }
        var hits = Physics.RaycastAll(ray);
        foreach (var hit in hits) {
            var cell = hit.collider.GetComponent<Cell>();
            if (cell) return cell;
        }
        return null;
    }

    void OnMouseDown() {
        if (controller.isPause) return;

        var hand = GameObject.FindObjectOfType<Hand>();
        if (isNip && hand && transform.parent.GetComponent<INip>().CanDrag) {
            draggable = true;
            Destroy(hand.gameObject);
        }

        if (!draggable) return;
        dragged = true;
    }

    // required for items which dragged directly
    void OnMouseUp() {
        if (!draggable) return;

        dragged = false;
        var cell = Hit();
        OnCellDrop(cell);
    }

    void OnCollisionEnter(Collision other) {
        if (!controller) return;
        if (!controller.gameInitialized) return;
        if (controller.isPause) return;

        var collider = other.gameObject.GetComponent<Collider>();
        if (collider && collider.dragged) return;
        if (isNip && draggable) return;

        var nip = this.transform.parent.GetComponent<INip>();
        if (nip != null) {
            nip.OnCollision(other);
        }
    }
}
