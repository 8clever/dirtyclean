using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopController : MonoBehaviour
{
    public List<ItemMain> Items = new List<ItemMain>();
    public  BuyItem Prefab;
    public GameObject Container;
    public Animator Animator;
    private GameController Controller;
    private void Awake () {
        var scene = SceneManager.GetActiveScene();
        var objects = scene.GetRootGameObjects();
        foreach (var o in objects) {
            var controller = o.GetComponent<GameController>();
            if (controller) {
                Controller = controller;
            }
        }
    }
    private void Start () {
        foreach (var i in Items) {
            var obj = Instantiate(Prefab, Container.transform);
            obj.item = i;
            obj.controller = Controller;
            obj.animator = Animator;
        }
    }
}
