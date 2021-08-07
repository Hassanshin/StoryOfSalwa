using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharUI : MonoBehaviour
{
    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private TextMeshProUGUI floatingText;

    public void SetHealth(float amount)
    {
        healthBar.fillAmount = amount;
    }

    public void SetFloatingText(float num)
    {
        floatingText.text = num < 0 ? $"{num}" : $"+{num}";
    }
}