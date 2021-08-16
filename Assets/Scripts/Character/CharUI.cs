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

    private int floatingId = 0;
    private Vector3 defaultPos;

    public void SetHealth(float amount)
    {
        healthBar.fillAmount = amount;

        defaultPos = floatingText.rectTransform.anchoredPosition;
    }

    private void animatefloatingText(string text)
    {
        if (LeanTween.isTweening(floatingId))
        {
            LeanTween.cancel(floatingId);
        }

        floatingText.text = $"{text}";

        floatingText.rectTransform.anchoredPosition = new Vector3(defaultPos.x, defaultPos.y - 0.5f, defaultPos.z);
        floatingId = LeanTween.moveY
            (floatingText.rectTransform, defaultPos.y, 2f).setEaseOutQuint()
            .setOnComplete(() =>
            {
                floatingText.text = $"";
                //floatingText.rectTransform.anchoredPosition = defaultPos;
            }).id;
    }

    public void SetFloatingText(float num)
    {
        animatefloatingText(num < 0 ? $"{num}" : $"+{num}");
    }

    public void SetFloatingText(string text)
    {
        animatefloatingText($"{text}");
    }
}