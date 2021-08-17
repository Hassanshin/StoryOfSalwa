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

        float mult = effectiveness(elemType, target.ElemType);

        target.IncreaseHealth(-(damage * mult));
    }

    private float effectiveness(ElementType user, ElementType target)
    {
        if (
            (user == ElementType.Fire && target == ElementType.Wind) ||
            (user == ElementType.Wind && target == ElementType.Watr) ||
            (user == ElementType.Watr && target == ElementType.Fire))
        {
            return 1.5f;
        }
        else if (
            (user == ElementType.Wind && target == ElementType.Fire) ||
            (user == ElementType.Watr && target == ElementType.Wind) ||
            (user == ElementType.Fire && target == ElementType.Watr))
        {
            return 0.5f;
        }

        return 1f;
    }
}