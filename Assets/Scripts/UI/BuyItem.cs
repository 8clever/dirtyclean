using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BuyItem : MonoBehaviour
{
    [Header("Required fields")]
    public ItemMain item;

    [Header("Prefab GameObjects")]
    public Image cell;
    public Text text;
    public Button button;

    [Header("Optional fields")]
    public Animator animator;
    private GameController controller;

    private void Awake () {
        var scene = SceneManager.GetActiveScene();
        var objects = scene.GetRootGameObjects();
        foreach (var o in objects) {
            var controller = o.GetComponent<GameController>();
            if (controller) {
                this.controller = controller;
            }
        }
        var sprite = item.GetComponent<SpriteRenderer>();
        text.text = item.Price.ToString();
        cell.sprite = sprite.sprite;
    }
    private void Update() {
        if (controller) {
            button.interactable = controller.point >= item.Price;
        }
    }

    public void OnClickBuy () {
        if (controller) {
            GameController.CreateItem(item);
            controller.AddPointsToPoints(-item.Price);
        }
        if (animator) {
            animator.Play("HIDE");
        }
    }
}
