using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Atk Card", menuName = "Salwa/Atk Card")]

public class CardDataAtk : CardData
{
    [Header("Stats")]
    public float damage = 100;

    public CardDataAtk()
    {
        type = CardType.Atk;
    }

    public override void Action(BaseChar target)
    {
        base.Action(target);
        AudioManager.Instance.PlaySfx(3);
        target.DecreaseHealth(damage);
    }
}