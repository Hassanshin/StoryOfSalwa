using System;
using System.Collections;
using UnityEngine;

public class PlayerChar : BaseChar
{
    public override void Attacking(BaseChar target, CardData cardData)
    {
        // animating
        base.Attacking(target, cardData);

        GameUI.Instance.Deck.StateActive(false);
    }

    public void AttackPhase()
    {
        AudioManager.Instance.PlaySfx(1);

        GameUI.Instance.Deck.StateActive();
    }

    public override void Die()
    {
        base.Die();
        GameManager.Instance.Level.LoseCheck();
    }
}