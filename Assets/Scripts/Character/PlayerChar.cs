using System;
using System.Collections;
using UnityEngine;

public class PlayerChar : BaseChar
{
    public override void Attacking(BaseChar target, CardData cardData)
    {
        // animating
        base.Attacking(target, cardData);

        StartCoroutine(GameManager.Instance.Deck.UsedCard(cardData));
    }

    public override void TurnPhase()
    {
        base.TurnPhase();
        GameManager.Instance.Deck.DeckActive();
    }

    public void EndTurnButton()
    {
        TurnManager.Instance.NextTurn();
        GameManager.Instance.Deck.DeckActive(false);
    }

    public override void Die()
    {
        base.Die();
        GameManager.Instance.Level.LoseCheck();
    }
}