﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BuyItem : MonoBehaviour
{
    public string resource;
    public Image cell;
    public Text text;
    public int price;

    public Button button;

    public GameController controller;

    public Animator animator;

    private void Awake () {
        var scene = SceneManager.GetActiveScene();
        var objects = scene.GetRootGameObjects();
        foreach (var o in objects) {
            var controller = o.GetComponent<GameController>();
            if (controller) {
                this.controller = controller;
            }
        }
    }
    private void Start() {
        var obj = Resources.Load<GameObject>(resource);
        var sprite = obj.GetComponent<SpriteRenderer>();
        text.text = price.ToString();
        cell.sprite = sprite.sprite;
        if (controller) {
            button.interactable = controller.point >= price;
            controller.AddPointsToPoints(-price);
        }
    }

    public void OnClickBuy () {
        if (controller) {
            controller.CreateItem(resource);
        }
        animator.Play("HIDE");
    }
}
