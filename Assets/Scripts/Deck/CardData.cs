using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : ScriptableObject
{
    public CardType type;
    public string cardName = "New Card";
    public Sprite sprite = null;

    public virtual void Action(BaseChar target)
    {
        //Debug.Log($" {type}Action {cardName}");
    }
}

public enum CardType { Atk, Ult, Sup }