using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField]
    private List<CardData> cardList = new List<CardData>();

    [Header("In Game")]
    [SerializeField]
    private List<CardData> deckCards = new List<CardData>();

    [SerializeField]
    private List<CardData> handCards = new List<CardData>();

    [SerializeField]
    private List<CardData> graveCards = new List<CardData>();

    [Header("Components")]
    [SerializeField]
    private DeckUI ui;

    #region CARD BASE MECHANIC

    public IEnumerator loadCardList()
    {
        ui.Initialize();

        deckCards.AddRange(cardList);
        yield return shuffleDeck();
    }

    private IEnumerator shuffleDeck()
    {
        deckCards.AddRange(graveCards);
        graveCards.Clear();

        yield return ShuffleDeck();
    }

    private IEnumerator ShuffleDeck()
    {
        List<CardData> randomized = new List<CardData>();
        randomized = deckCards.OrderBy(i => Guid.NewGuid()).ToList();
        deckCards = randomized;

        yield return null;
    }

    private IEnumerator generateHandCard()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return drawCard(i);
        }
        ui.StateActive();
    }

    private IEnumerator clearHand()
    {
        ui.StateActive(false);
        for (int i = 0; i < handCards.Count; i++)
        {
            if (handCards[i] != null)
            {
                yield return ui.applyUsedCard(i);
                graveCards.Add(handCards[i]);
            }
        }
        handCards.Clear();

        yield return null;
    }

    private IEnumerator drawCard(int index)
    {
        if (deckCards.Count <= 0)
        {
            yield return new WaitForSeconds(1f);
            yield return shuffleDeck();
        }
        CardData card = deckCards[0];

        handCards.Add(card);
        deckCards.Remove(card);

        yield return ui.applyHandCard(index, card);
    }

    public IEnumerator UsedCard(CardData card)
    {
        int index = handCards.IndexOf(card);// handCards.FindIndex((a) => card == a);
        handCards[index] = null;
        graveCards.Add(card);

        yield return ui.applyUsedCard(index);
    }

    #endregion CARD BASE MECHANIC

    public void ClearDeck()
    {
        ui.ClearDeck();

        deckCards.Clear();
        handCards.Clear();
        graveCards.Clear();
    }

    public void DeckActive(bool _state = true)
    {
        if (_state)
        {
            StartCoroutine(generateHandCard());
        }
        else
        {
            StartCoroutine(clearHand());
        }
    }
}