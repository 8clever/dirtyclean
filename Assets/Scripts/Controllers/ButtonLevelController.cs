using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonLevelController : MonoBehaviour
{
    public static readonly string GreenColor = "#A5EC79";
    public static readonly string GrayColor = "#FFFFFF";
    public Scenes RequiredLevel;
    void Awake()
    {
        var btn = GetComponent<Button>();
        var colors = btn.colors;
        var lvlIsCompleted = PlayerPrefs.HasKey(RequiredLevel.ToString());
        var colorHex = lvlIsCompleted ? GreenColor : GrayColor;
        Color color;
        ColorUtility.TryParseHtmlString(colorHex, out color);
        colors.normalColor = color;
        colors.highlightedColor = color;
        btn.colors = colors;
        btn.interactable = lvlIsCompleted;
    }
}
