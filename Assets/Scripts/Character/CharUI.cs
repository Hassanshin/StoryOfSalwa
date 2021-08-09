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

    private LTDescr descr = null;
    private Vector3 defaultPos;

    public void SetHealth(float amount)
    {
        healthBar.fillAmount = amount;

        defaultPos = floatingText.rectTransform.anchoredPosition;
    }

    public void SetFloatingText(float num)
    {
        if (descr != null && LeanTween.isTweening(descr.id))
        {
            LeanTween.cancel(descr.id);
        }

        floatingText.rectTransform.anchoredPosition = new Vector3(defaultPos.x, defaultPos.y - 0.5f, defaultPos.z);

        floatingText.text = num < 0 ? $"{num}" : $"+{num}";
        descr = LeanTween.moveY
            (floatingText.rectTransform, defaultPos.y, 2f)
            .setOnComplete(() =>
            {
                floatingText.text = "";
                //floatingText.rectTransform.anchoredPosition = defaultPos;
            })
            .setEaseOutQuint();
    }
}