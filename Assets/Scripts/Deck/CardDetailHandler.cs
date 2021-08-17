using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CardDetailHandler : MonoBehaviour
{
    private CardData data;

    public CardData ViewData
    {
        get => data;
        set
        {
            data = value;

            apply();
        }
    }

    [Header("Panel Card kiri")]
    [SerializeField]
    private Image borderCard;

    [SerializeField]
    private Image sprite;

    [SerializeField]
    private TextMeshProUGUI cardJudul;

    [SerializeField]
    private TextMeshProUGUI cardDamage;

    [Header("Panel kanan")]
    [SerializeField]
    private TextMeshProUGUI judul;

    [SerializeField]
    private TextMeshProUGUI penjelasan;

    [SerializeField]
    private TextMeshProUGUI element;

    [Header("komponen")]
    [SerializeField]
    private Canvas panel;

    [SerializeField]
    private Color[] typeColor = new Color[3];

    [SerializeField]
    private string[] elemText = new string[3];

    [SerializeField]
    private Button closeBtn;

    private void apply()
    {
        sprite.sprite = data.sprite;
        cardJudul.text = data.name;
        judul.text = data.name;

        borderCard.color = typeColor[(int)data.type];
        element.text = elemText[(int)data.elemType];

        penjelasan.text = data.penjelasan;

        if (data is CardDataAtk)
        {
            CardDataAtk atkData = (CardDataAtk)data;
            cardDamage.text = $"{ atkData.damage}";
        }
        else
        {
            CardDataSup atkData = (CardDataSup)data;
            cardDamage.text = $"{ atkData.healAmount}";
        }

        panel.gameObject.SetActive(true);
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(() =>
        {
            panel.gameObject.SetActive(false);
        });
    }
}