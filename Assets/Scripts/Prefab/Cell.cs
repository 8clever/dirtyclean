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

    void Start()
    {
        current = this.GetComponent<Image>();
        empty = current.color;
        canDrop = current.color;
        canDrop.a = current.color.a * 3;
        cantDrop = Color.red;
        cantDrop.a = current.color.a * 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (current.color != empty) {
            current.color = empty;
        }
    }
}
