using System.Collections;

using UnityEngine;

[CreateAssetMenu(fileName = "New Atk Card", menuName = "Salwa/Atk Card")]

public class CardDataAtk : CardData
{
    [Header("Atk Stats", order = 0)]
    public float totalDamage = 100;
    public int totalAtk = 1;
    public bool lastDamageIncrease = false;

    public CardDataAtk()
    {
        type = CardType.Atk;
    }

    public override void Action(BaseChar target)
    {
        base.Action(target);

        target.TakeDamage(this);
    }
}