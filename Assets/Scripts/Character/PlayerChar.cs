using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChar : BaseChar
{
    public void DeckAttacking(BaseChar target, CardData cardData, int cardIndex)
    {
        Attacking(target, cardData);
        StartCoroutine(GameManager.Instance.Deck.UsedCard(cardData, cardIndex));
    }

    protected override void hit(BaseChar target, CardData cardData, bool willHit)
    {
        if (cardData is CardDataAtk)
        {
            CardDataAtk atk = (CardDataAtk)cardData;
            List<BaseChar> targets = GameManager.Instance.Level.GetEnemiesInRange(target, atk.HitArea);

            foreach (BaseChar baseChar in targets)
            {
                if (!baseChar.IsDie)
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