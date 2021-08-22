using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBank : MonoBehaviour
{
    [SerializeField]
    private List<CardData> allCards = new List<CardData>();

    public List<CardData> AllCards => allCards;

    [SerializeField]
    private List<CardData> defaultCards = new List<CardData>();

    public List<CardData> DefaultCard => defaultCards;

    public CardData GetCard(string name)
    {
        CardData x = allCards.Find((a) => a.name == name);

        if (x == null)
        {
            Debug.Log($"Cannot find card with name {name}");
            return null;
        }

        return allCards.Find((a) => a.name == name);
    }
}