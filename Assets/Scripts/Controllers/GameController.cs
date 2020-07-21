﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    
    private int step = 0;

    private int health = 400;

    private GameObject shadow;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DefaultStart () {
        ToggleShadow(false);
        RenderHealth();
        GenerateItem();
        var cell = GetRandomRespawnCell();
        if (cell) {
            Instantiate(Resources.Load<Dvornik>(Dvornik.resourcePath), cell.transform);
        }
    }

    private IEnumerator MoveNips () {
        var persons = GameObject.FindObjectsOfType<Nip>();
        foreach (var p in persons) {
            (p as INip).NextStep();
            yield return new WaitForSeconds(0);
        }
    }

    public int CurrentTime () {
        var time = step % 8;
        var dayTime = GameObject.Find("DayTime");
        if (dayTime == null) throw new System.Exception("DayTime not exist");

        var indicator = time < 4 ? "sun" : "moon";
        var indicatorId = (step % 4) + 1;
        var indicatorPath = $"Indicators/{indicator}{indicatorId}";
        var sprite = Resources.Load<Sprite>(indicatorPath);
        if (sprite == null) throw new System.Exception($"{indicatorPath} not found");
        
        dayTime.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        return step % 8;
    }

    private void RenderHealth () {
        var healthObject = GameObject.Find("Health").GetComponent<Text>();
        if (healthObject == null) throw new System.Exception("Health not exists");
        healthObject.text = health.ToString();
    }

    private void IsGameOver () {
        var cells = GameObject.FindGameObjectsWithTag("cell");
        var stepsExists = false;
        foreach (var cell in cells) {
            if (cell.transform.childCount == 0) {
                stepsExists = true;
                break;
            }
        }
        if (stepsExists && health > 0) return;
        SceneController.LoadScene(SceneController.GameOverScene);
    }

    private void ToggleShadow (bool active) {
        shadow = shadow ? shadow : GameObject.Find("Shadow");
        if (shadow == null) throw new System.Exception("Shadow GameObject is required");
        shadow.SetActive(active);
    }

    public void NextStep () {
        IsGameOver();

        step += 1;
        health -= 1;

        RenderHealth();
        GenerateItem();
        
        var time = CurrentTime();
        
        if (time == 0) {
            ToggleShadow(false);
            var cell = GetRandomRespawnCell();
            if (cell) {
                Instantiate(Resources.Load<Dvornik>(Dvornik.resourcePath), cell.transform);
            }
        }
        if (time == 4) {
            ToggleShadow(true);
            var cell = GetRandomRespawnCell();
            if (cell) {
                Instantiate(Resources.Load<StreetDog>(StreetDog.resourcePath), cell.transform);
            }
        }
        
        StartCoroutine(MoveNips());
    }

    public void GenerateItem () {
        var chance = Random.Range(0, 100);
        var itemField = GameObject.Find("ItemField");
        if (itemField == null) throw new System.Exception("ItemField is required");
        if (chance > 50) {
            Instantiate(Resources.Load("Item/Hand"), itemField.transform);
            return;
        }
        Instantiate(Resources.Load("Item/Mayka"), itemField.transform);
    }

    public Cell GetRandomRespawnCell () {
        var objects = GameObject.FindGameObjectsWithTag("cell");
        var emptyCells = new List<Cell>();
        foreach(var obj in objects) {
            var cell = obj.GetComponent<Cell>();
            if (cell && cell.isRespawn && cell.transform.childCount == 0) {
                emptyCells.Add(cell);
            }
        }
        var idx = Random.Range(0, emptyCells.Count);
        return emptyCells[idx];
    }

    public void AddPointsToHealth (int num) {
        health += num;
    }
}
