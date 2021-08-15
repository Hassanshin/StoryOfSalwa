using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : ScriptableObject
{
    public CardType type;

    public Sprite sprite = null;

    [Header("Instant Effect", order = 2)]
    public float healAmount = 0;
    public bool stun = false;
    public bool cure = false;

    [Header("Buff Stats", order = 2)]
    [Range(-30, 30)] public float speedAmount = 0;
    [Range(-30, 30)] public float accuracyAmount = 0;
    [Range(-30, 30)] public float evasionAmount = 0;
    public float damagePerSecondAmount = 0;

    [Header("Buff Duration", order = 2)]
    public int buffDuration = 1;

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
        if (damagePerSecondAmount > 0)
        {
            Buff _buff = new Buff("Regen", BuffType.dps, buffDuration, damagePerSecondAmount);
            target.AddBuff(_buff);
        }
        else if (damagePerSecondAmount < 0)
        {
            Buff _buff = new Buff("Bleed", BuffType.dps, buffDuration, damagePerSecondAmount);
            target.AddBuff(_buff);
        }
    }

    private void instantBuff(BaseChar target)
    {
        if (stun)
        {
            Buff _buff = new Buff("Stun", BuffType.stun, buffDuration);
            target.AddBuff(_buff);
        }
        if (cure)
        {
            Buff _buff = new Buff("Cure", BuffType.dispel, buffDuration);
            target.AddBuff(_buff);
        }
    }

    private void speedBuff(BaseChar target)
    {
        if (speedAmount > 0)
        {
            Buff _buff = new Buff("Boost", BuffType.speed, buffDuration, speedAmount);
            target.AddBuff(_buff);
        }
        else if (speedAmount < 0)
        {
            Buff _buff = new Buff("Slow", BuffType.speed, buffDuration, speedAmount);
            target.AddBuff(_buff);
        }
    }

    private void accuracyBuff(BaseChar target)
    {
        if (accuracyAmount > 0)
        {
            Buff _buff = new Buff("Precision", BuffType.acc, buffDuration, accuracyAmount);
            target.AddBuff(_buff);
        }
        else if (accuracyAmount < 0)
        {
            Buff _buff = new Buff("Blind", BuffType.acc, buffDuration, accuracyAmount);
            target.AddBuff(_buff);
        }
    }

    private void evasionBuff(BaseChar target)
    {
        if (evasionAmount > 0)
        {
            Buff _buff = new Buff("Swift", BuffType.eva, buffDuration, evasionAmount);
            target.AddBuff(_buff);
        }
        else if (evasionAmount < 0)
        {
            Buff _buff = new Buff("Sloppy", BuffType.eva, buffDuration, evasionAmount);
            target.AddBuff(_buff);
        }
    }
}

public enum CardType { Atk, Ult, Sup }