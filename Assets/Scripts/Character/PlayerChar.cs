using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChar : BaseChar
{
    public override void Attacking(BaseChar target, CardData cardData)
    {
        // animating
        base.Attacking(target, cardData);

        StartCoroutine(GameManager.Instance.Deck.UsedCard(cardData));
    }

    protected override void hit(BaseChar target, CardData cardData, bool willHit)
    {
        if (cardData is CardDataAtk)
        {
            CardDataAtk atk = (CardDataAtk)cardData;
            List<BaseChar> targets = GameManager.Instance.Level.GetEnemiesInRange(target, atk.HitArea);

            foreach (BaseChar baseChar in targets)
            {
                base.hit(baseChar, cardData, willHit);
            }
        }
        else
        {
            base.hit(target, cardData, willHit);
        }
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