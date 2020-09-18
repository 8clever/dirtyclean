using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isRespawn = false;

    public bool isPrison = false;

    public Color empty;

    public Color canDrop;

    public Color cantDrop;

    public Image current;

    public Config config;

    public bool isBlocker;

    void Start()
    {
        isBlocker = GetComponentInChildren<Blocker>() != null;
        config = Config.GetConfig();
        current = GetComponent<Image>();
        empty = current.color;
        canDrop = current.color;
        canDrop.a = current.color.a * 3;
        cantDrop = Color.red;
        cantDrop.a = current.color.a * 3;
    }

    // Update is called once per frame
    void Update()
    {
        // change color on mouse hover
        if (current.color != empty) {
            current.color = empty;
        }
        // only for blockers
        if (isBlocker) {
            if (current.enabled) {
                current.enabled = false;
            }
            return;
        }
        // for all active cells
        if (config.GameFieldWeb != current.enabled) {
            current.enabled = config.GameFieldWeb;
        }
    }
}
