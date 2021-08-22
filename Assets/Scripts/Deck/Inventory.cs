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

    [SerializeField]
    private UnityEngine.UI.Button backBtn;

    [Header("CardBank")]
    public CardBank cardBank;

    public bool DeckIsFine
    {
        get
        {
            if (dDeck.cards.Count == 20)
            {
                StartCoroutine(saveDeck());
                return true;
            }
            else
            {
                Debug.Log("Deck is bad");
                return false;
            }
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

        backBtn.onClick.AddListener(() => BackMainMenu());
    }

    public IEnumerator Initialize()
    {
        yield return loadDeck();

        yield return spawn(dBag);
        yield return spawn(dDeck);
        yield return spawn(dTrunk);
    }

    private IEnumerator loadDeck()
    {
        // Load Card

        //bag.cards.AddRange(cardBank.DefaultCard);

        if (SaveManager.Instance.IsNewPlayer)
        {
            dDeck.cards.AddRange(cardBank.DefaultCard);
            dTrunk.cards.AddRange(cardBank.TrunkCards);
            yield return saveDeck();
        }
        else
        {
            dDeck.cards.AddRange(cardBank.GetCards(SaveManager.Instance.playerData.cardSave.DeckCards));
            dTrunk.cards.AddRange(cardBank.GetCards(SaveManager.Instance.playerData.cardSave.TrunkCards));
            dBag.cards.AddRange(cardBank.GetCards(SaveManager.Instance.playerData.cardSave.BagCards));
        }

        yield return null;
    }

    private IEnumerator saveDeck()
    {
        List<string> dDeckSave = new List<string>();
        foreach (CardData item in dDeck.cards)
        {
            dDeckSave.Add(item.name);
        }

        SaveManager.Instance.playerData.cardSave.DeckCards = dDeckSave;

        List<string> dTrunkSave = new List<string>();
        foreach (CardData item in dTrunk.cards)
        {
            dTrunkSave.Add(item.name);
        }

        SaveManager.Instance.playerData.cardSave.TrunkCards = dTrunkSave;

        List<string> dBagSave = new List<string>();
        foreach (CardData item in dBag.cards)
        {
            dBagSave.Add(item.name);
        }

        SaveManager.Instance.playerData.cardSave.BagCards = dBagSave;

        SaveManager.Instance.Save();

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
        SaveManager.Instance.playerData.cardSave.BagCards.Add(a.name);
    }

    #endregion Drag Drop Card

    public void BackMainMenu()
    {
        if (DeckIsFine)
        {
            MainMenuUI.Instance.DisplayOnly(0);
        }
        else
        {
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