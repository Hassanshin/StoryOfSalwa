using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : ScriptableObject
{
    public CardType type;

    public Sprite sprite = null;

    [Header("BUFF DATA POP")]
    public BuffCardData buffData;

    public virtual void Action(BaseChar target)
    {
        instantBuff(target);

        speedBuff(target);
        accuracyBuff(target);
        evasionBuff(target);
        dpsBuff(target);
    }

    private void dpsBuff(BaseChar target)
    {
        if (buffData.damagePerSecondAmount > 0)
        {
            Buff _buff = new Buff("Regen", BuffType.dps, buffData.buffDuration, buffData.damagePerSecondAmount);
            target.AddBuff(_buff);
        }
        else if (buffData.damagePerSecondAmount < 0)
        {
            Buff _buff = new Buff("Bleed", BuffType.dps, buffData.buffDuration, buffData.damagePerSecondAmount);
            target.AddBuff(_buff);
        }
    }

    private void instantBuff(BaseChar target)
    {
        if (buffData.stun)
        {
            Buff _buff = new Buff("Stun", BuffType.stun, buffData.buffDuration);
            target.AddBuff(_buff);
        }
        if (buffData.cure)
        {
            Buff _buff = new Buff("Cure", BuffType.dispel, buffData.buffDuration);
            target.AddBuff(_buff);
        }
    }

    private void speedBuff(BaseChar target)
    {
        if (buffData.speedAmount > 0)
        {
            Buff _buff = new Buff("Boost", BuffType.speed, buffData.buffDuration, buffData.speedAmount);
            target.AddBuff(_buff);
        }
        else if (buffData.speedAmount < 0)
        {
            Buff _buff = new Buff("Slow", BuffType.speed, buffData.buffDuration, buffData.speedAmount);
            target.AddBuff(_buff);
        }
    }

    private void accuracyBuff(BaseChar target)
    {
        if (buffData.accuracyAmount > 0)
        {
            Buff _buff = new Buff("Precision", BuffType.acc, buffData.buffDuration, buffData.accuracyAmount);
            target.AddBuff(_buff);
        }
        else if (buffData.accuracyAmount < 0)
        {
            Buff _buff = new Buff("Blind", BuffType.acc, buffData.buffDuration, buffData.accuracyAmount);
            target.AddBuff(_buff);
        }
    }

    private void evasionBuff(BaseChar target)
    {
        if (buffData.evasionAmount > 0)
        {
            Buff _buff = new Buff("Swift", BuffType.eva, buffData.buffDuration, buffData.evasionAmount);
            target.AddBuff(_buff);
        }
        else if (buffData.evasionAmount < 0)
        {
            Buff _buff = new Buff("Sloppy", BuffType.eva, buffData.buffDuration, buffData.evasionAmount);
            target.AddBuff(_buff);
        }
    }
}

public enum CardType { Atk, Ult, Sup }