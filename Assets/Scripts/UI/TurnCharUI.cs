using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnCharUI : MonoBehaviour
{
    private BaseChar curChar;

    public BaseChar data
    {
        get
        {
            return curChar;
        }
        set
        {
            curChar = value;
            charName.text = curChar.name;
        }
    }

    [SerializeField]
    private TextMeshProUGUI charName;
    public TextMeshProUGUI CharName { get => charName; }

    [SerializeField]
    private Image icon;

    public Image Icon { get => icon; }

    public RectTransform rect
    {
        get => GetComponent<RectTransform>();
    }

    public float curYPos
    {
        get => rect.anchoredPosition.y;
    }
}