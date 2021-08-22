using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    [SerializeField]
    private CardPile dBag;

    [SerializeField]
    private CardPile dTrunk;

    [SerializeField]
    private CardPile dDeck;

    [Header("UI")]
    public GameObject cardUiPrefab;

    [Header("CardBank")]
    public CardBank cardBank;

    public bool DeckIsFine
    {
        get
        {
            if (dTrunk.cards.Count == 5 && dDeck.cards.Count == 20)
            {
                return true;
            }

            return false;
        }
    }

    public List<CardData> DeckCards
    {
        get
        {
            return dDeck.cards;
        }
    }

    public List<CardData> TrunkCards
    {
        get
        {
            return dTrunk.cards;
        }
    }

    public override void Initialization()
    {
        dBag.type = PileType.bag;
        dDeck.type = PileType.deck;
        dTrunk.type = PileType.trunk;

        cardBank = GetComponent<CardBank>();
        StartCoroutine(Initialize());
    }

    public IEnumerator Initialize()
    {
        yield return loadCard();

        yield return spawn(dBag);
        yield return spawn(dDeck);
        yield return spawn(dTrunk);
    }

    private IEnumerator loadCard()
    {
        // Load Card

        //bag.cards.AddRange(cardBank.DefaultCard);
        dDeck.cards.AddRange(cardBank.DefaultCard);

        yield return null;
    }

    #region UI

    private IEnumerator spawn(CardPile pile)
    {
        for (int i = 0; i < pile.cards.Count; i++)
        {
            CardUI spawn = Instantiate(cardUiPrefab, pile.pivotCard).GetComponent<CardUI>();

            pile.cardUI.Add(spawn);
            spawn.SetCardData(pile.cards[i]);
        }

        yield return null;
    }

    #endregion UI

    #region Drag Drop Card

    public void Swap(CardUI a, CardUI b)
    {
        Debug.Log($"Swap: {a.Data.name}: - {b.Data.name}:");

        CardData c = a.Data;

        a.SetCardData(b.Data);
        b.SetCardData(c);

        // SWAP SYSTEM
        StartCoroutine(refreshAllPile());
    }

    private IEnumerator refreshAllPile()
    {
        yield return dBag.Refresh();
        yield return dTrunk.Refresh();
        yield return dDeck.Refresh();
    }

    public void AddNewCard(CardData a)
    {
        Debug.Log($"Added: {a.name}");
        dBag.cards.Insert(0, a);

        CardUI newCard = Instantiate(cardUiPrefab, dBag.pivotCard).GetComponent<CardUI>();
        newCard.SetCardData(a);
        newCard.transform.SetAsFirstSibling();

        dBag.cardUI.Add(newCard);
    }

    #endregion Drag Drop Card

    public bool QuitSaveInventory()
    {
        if (DeckIsFine)
        {
            // save
            return true;
        }
        else
        {
            Debug.Log("Deck is bad");
            return false;
        }
    }
}

[System.Serializable]
public class CardPile
{
    public PileType type;
    public List<CardUI> cardUI = new List<CardUI>();

    public List<CardData> cards = new List<CardData>();

    [Header("Components")]
    public Transform pivotCard;

    public IEnumerator Refresh()
    {
        cards.Clear();
        for (int i = 0; i < cardUI.Count; i++)
        {
            cards.Add(cardUI[i].Data);
        }
        yield return null;
    }
}