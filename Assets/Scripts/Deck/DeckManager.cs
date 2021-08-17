using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField]
    private List<CardData> cardList = new List<CardData>();

    [SerializeField]
    private gameCards inGameDeck;

    [Header("Components")]
    [SerializeField]
    private DeckUI ui;

    private Coroutine process = null;

    #region CARD BASE MECHANIC

    public IEnumerator loadCardList()
    {
        ui.Initialize();

        inGameDeck.deck.AddRange(cardList);

        yield return shuffleDeck();
    }

    private IEnumerator shuffleDeck()
    {
        inGameDeck.deck.AddRange(inGameDeck.grave);
        inGameDeck.grave.Clear();

        List<CardData> randomized = new List<CardData>();
        randomized = inGameDeck.deck.OrderBy(i => Guid.NewGuid()).ToList();
        inGameDeck.deck = randomized;

        updateNumber();

        yield return null;
    }

    private IEnumerator generateHandCard()
    {
        for (int i = 0; i < inGameDeck.HandSize; i++)
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
        for (int i = 0; i < inGameDeck.HandSize; i++)
        {
            if (inGameDeck.hand[i] != null && !inGameDeck.handLock[i])
            {
                inGameDeck.grave.Add(inGameDeck.hand[i]);
                inGameDeck.hand[i] = null;

                yield return ui.applyUsedCard(i);
                updateNumber();
            }
        }

        process = null;
    }

    private IEnumerator drawCard(int index)
    {
        if (inGameDeck.hand[index] != null) { yield break; }

        if (inGameDeck.deck.Count <= 0)
        {
            ui.SetTopText("Shuffling");

            // animate shuffling
            yield return new WaitForSeconds(1f);
            yield return shuffleDeck();

            ui.SetTopText("Your Turn");
        }
        CardData card = inGameDeck.deck[0];

        inGameDeck.AddHandCard(card);
        inGameDeck.deck.Remove(card);

        updateNumber();

        yield return ui.applyHandCard(index, card);
    }

    private void updateNumber()
    {
        ui.UpdateNumber(inGameDeck.deck.Count, inGameDeck.grave.Count);
    }

    public IEnumerator UsedCard(CardData card)
    {
        int index = Array.IndexOf(inGameDeck.hand, card);

        inGameDeck.hand[index] = null;
        inGameDeck.grave.Add(card);

        updateNumber();

        yield return ui.applyUsedCard(index);
    }

    #endregion CARD BASE MECHANIC

    public void LockHandCard(int index, bool state)
    {
        inGameDeck.Lock(index, state);
        ui.UpdatePadlock(state, inGameDeck.handLock);
    }

    public void ClearDeck()
    {
        ui.StateActive(false);
        ui.ClearDeck();
        if (process != null)
        {
            StopCoroutine(process);
            process = null;
        }
        inGameDeck = new gameCards();
    }

    public void DeckActive(bool _state = true)
    {
        if (process != null) { return; }

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

[System.Serializable]
public class gameCards
{
    public List<CardData> deck = new List<CardData>();

    public CardData[] hand = new CardData[5];
    public bool[] handLock = new bool[5];
    public int HandSize = 5;

    public List<CardData> grave = new List<CardData>();

    private int getLastIndex
    {
        get
        {
            for (int i = 0; i < HandSize; i++)
            {
                if (hand[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }
    }

    public void AddHandCard(CardData card)
    {
        hand[getLastIndex] = card;
    }

    public void Lock(int index, bool state)
    {
        for (int i = 0; i < handLock.Length; i++)
        {
            handLock[i] = false;
        }
        handLock[index] = state;
    }
}