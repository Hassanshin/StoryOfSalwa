using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : ScriptableObject
{
    public CardType type;

    public Sprite sprite = null;

    [Header("Atk Stats")]
    public float damage = 100;
    public int totalAtk = 1;

    [Header("Buff Stats")]
    public float healAmount = 0;
    public float speedAmount = 0;
    public float accuracyAmount = 0;
    public float evasionAmount = 0;

    public int buffDuration = 1;

    public virtual void Action(BaseChar target)
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
        //_buff.mAmount = speedAmount;
    }

    //Debug.Log($" {type}Action {cardName}");
}

public enum CardType { Atk, Ult, Sup }