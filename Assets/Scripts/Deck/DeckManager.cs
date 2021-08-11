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

    private Coroutine process = null;

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

        List<CardData> randomized = new List<CardData>();
        randomized = deckCards.OrderBy(i => Guid.NewGuid()).ToList();
        deckCards = randomized;

        updateNumber();

        yield return null;
    }

    private IEnumerator generateHandCard()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return drawCard(i);
        }

        AudioManager.Instance.PlaySfx(1);

        ui.StateActive();

        process = null;
    }

    private IEnumerator clearHand()
    {
        ui.StateActive(false);
        for (int i = 0; i < handCards.Count; i++)
        {
            if (handCards[i] != null)
            {
                graveCards.Add(handCards[i]);
                yield return ui.applyUsedCard(i);
                Debug.Log($" is null? :{handCards[i] != null}");
                updateNumber();
            }
        }

        handCards.Clear();
        Debug.Log($" handcards cleared");

        process = null;
    }

    private IEnumerator drawCard(int index)
    {
        if (deckCards.Count <= 0)
        {
            ui.SetTopText("Shuffling");

            // animate shuffling
            yield return new WaitForSeconds(1f);
            yield return shuffleDeck();

            ui.SetTopText("Your Turn");
        }
        CardData card = deckCards[0];

        handCards.Add(card);
        deckCards.Remove(card);

        updateNumber();

        yield return ui.applyHandCard(index, card);
    }

    private void updateNumber()
    {
        ui.UpdateNumber(deckCards.Count, graveCards.Count);
    }

    public IEnumerator UsedCard(CardData card)
    {
        int index = handCards.IndexOf(card);
        handCards[index] = null;
        graveCards.Add(card);

        updateNumber();

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
        //if (process != null) { return; }

        if (_state)
        {
            ui.SetTopText("Your Turn");
            process = StartCoroutine(generateHandCard());
        }
        else
        {
            ui.SetTopText("");
            process = StartCoroutine(clearHand());
        }
    }
}