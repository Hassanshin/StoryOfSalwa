using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Atk Card", menuName = "Salwa/Atk Card")]

public class CardDataAtk : CardData
{
    [Header("Atk Stats", order = 0)]
    public float damage = 100;
    public int totalAtk = 1;

    public CardDataAtk()
    {
        type = CardType.Atk;
    }

    public override void Action(BaseChar target)
    {
        base.Action(target);
        AudioManager.Instance.PlaySfx(3);
        target.IncreaseHealth(-damage);
    }
}