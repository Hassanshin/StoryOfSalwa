using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField]
    private Currency gold = new Currency();

    [SerializeField]
    private List<CardSell> cardSell = new List<CardSell>();

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI goldText;

    [SerializeField]
    private List<CardUI> cardSellUi = new List<CardUI>();

    public override void Initialization()
    {
        gold.OnValueChanged.AddListener((a) =>
        {
            goldText.text = $"{a}";
        });
        gold.Set(1000);

        setData();

        GameManager.Instance.Level.OnGameOver.AddListener((isWin) =>
        {
            if (isWin)
            {
                gold.Add(100);
            }
        });
    }

    [ContextMenu("Randomize")]
    private void setData()
    {
        List<CardData> all = Inventory.Instance.cardBank.AllCards;
        cardSell.Clear();
        for (int i = 0; i < cardSellUi.Count; i++)
        {
            CardData card = all[Random.Range(0, all.Count)]; // SAVE THE SHOP?
            CardSell generated = new CardSell(card, 100);
            cardSell.Add(generated);

            // UI
            cardSellUi[i].SetCardData(cardSell[i].card);
            Button buyBtn = cardSellUi[i].transform.GetChild(1).GetComponent<Button>();

            buyBtn.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = $"Buy {cardSell[i].price}";
            int copy = i;

            buyBtn.onClick.RemoveAllListeners();
            buyBtn.onClick.AddListener(() =>
            {
                buyBtnAction(copy);
            });
        }
    }

    private void buyBtnAction(int i)
    {
        buy(cardSell[i]);
    }

    private bool buy(CardSell _card)
    {
        if (gold.CurValue >= _card.price)
        {
            Inventory.Instance.AddNewCard(_card.card);
            gold.Add(-_card.price);
            return true;
        }
        else
        {
            Debug.Log("Not enough money");
            return false;
        }
    }
}

[System.Serializable]
public class CardSell
{
    public CardData card;
    public float price = 100;

    public CardSell(CardData card, int price)
    {
        this.card = card;
        this.price = price;
    }
}

[System.Serializable]
public class Currency
{
    [SerializeField]
    private float value;
    public float CurValue => value;

    public UnityEngine.Events.UnityEvent<float> OnValueChanged;

    public void Set(float a)
    {
        value = a;
        OnValueChanged?.Invoke(value);
    }

    public void Add(float a)
    {
        value += a;
        OnValueChanged?.Invoke(value);
    }
}