using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
public class Collider : MonoBehaviour
{
    public bool draggable = false;
    public AudioClip handPickedAudio;
    public AudioClip placeAudio;
    private bool dragged = false;
    private bool isNip;
    private bool isItem;
    private GameObject handPicked;
    private GameController controller;
    private Config config;
    public bool clickable = false;
    private GameObject clickableAnimation;
    private void Awake() {
        config = Config.GetConfig();
        controller = GameObject.FindObjectOfType<GameController>();
        isNip = transform.parent.GetComponent<Nip>() != null;
        isItem = transform.parent.GetComponent<Item>() != null;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isPause) return;

        if (handPicked && !dragged) {
            clickable = true;
        }

        if (clickable && !clickableAnimation) {
            var animation = Resources.Load<GameObject>("Animations/ClickableAnimation");
            var render = animation.GetComponent<SpriteRenderer>();
            render.sprite = gameObject.transform.parent.GetComponent<SpriteRenderer>().sprite;
            clickableAnimation = Instantiate(animation, transform);
        }

        if (!clickable && clickableAnimation) {
            Destroy(clickableAnimation.gameObject);
            clickableAnimation = null;
        }

        if (dragged) {
            OnCellCanDrop(Hit(true), true);
            var nips = FindObjectsOfType<Nip>();
            foreach (var n in nips) {
                var canDrop = OnCellCanDrop(n.GetComponentInParent<Cell>(), false);
                if (canDrop) {
                    n.GetComponentInChildren<Collider>().clickable = true;
                }
            }
        } 

        clickable = false;

        if (isNip) {
            if (draggable && handPicked == null) {
                handPicked = Instantiate(Resources.Load("Item/HandPicked"), this.transform.parent) as GameObject;
                MusicController.PlayOnce(handPickedAudio);
            }

            if (!draggable && handPicked != null) {
                Destroy(handPicked);
                handPicked = null;
            }
        }

        // required for items which placed directly on cell
        if (isItem && Input.GetMouseButtonDown(0) && dragged == false) {
            var parentName = transform.parent.name;
            if (parentName.Contains("Hand")) {
                var cell = Hit();
                if (cell && cell.transform.childCount == 0) {
                    OnMouseUp();
                }
                return;
            }
            OnMouseUp();
        }
    }

    void OnCellDrop (Cell cell) {
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
                    Instantiate(Resources.Load<Hand>(Hand.ResourcePath), itemField.transform);
                }
            }
            return;
        }
        
        if (isNip && cell.isPrison && cell.transform.childCount > 0) {
            MoveBackNip();
        }

        (item as IItem)?.OnDrop(cell.gameObject);
        (item as ItemMain)?.OnDrop(cell.gameObject);
        (nip as INip)?.OnDrop(cell.gameObject);
        (nip as NpcMain)?.OnDrop(cell.gameObject);
    }

    bool OnCellCanDrop (Cell cell, bool colored = false) {
        if (!cell) return false;

        var nip = GetComponentInParent<Nip>();
        var item = GetComponentInParent<Item>();
        var img = cell.GetComponent<Image>();
        bool canDrop = (
            (nip as INip)?.CanDrop(cell) ??
            (nip as NpcMain)?.CanDrop(cell) ??
            (item as IItem)?.CanDrop(cell) ??
            (item as ItemMain)?.CanDrop(cell) ??
            false
        );
        if (colored) {
            img.color = canDrop ? cell.canDrop : cell.cantDrop;
        }
        return canDrop;
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
        var nip = transform.parent.GetComponent<Nip>();
        var CanDrag = (nip as INip)?.CanDrag ?? (nip as NpcMain)?.CanDrag ?? false;
        if (isNip && hand && CanDrag) {
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
        if (OnCellCanDrop(cell)) {
            if (placeAudio) {
                MusicController.PlayOnce(placeAudio);
            }
        }
    }

    void OnCollisionEnter(Collision other) {
        if (!controller) return;
        if (!controller.gameInitialized) return;
        if (controller.isPause) return;

        var collider = other.gameObject.GetComponent<Collider>();
        if (collider && collider.dragged) return;
        if (isNip && draggable) return;

        var nip = this.transform.parent.GetComponent<Nip>();
        (nip as INip)?.OnCollision(other);
        (nip as NpcMain)?.OnCollision(other);
    }
}
