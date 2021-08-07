using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sup Card", menuName = "Salwa/Sup Card")]

public class CardDataSup : CardData
{
    [Header("Stats")]
    public float healAmount = 100;
    public float speedAmount = 5;

    public CardDataSup()
    {
        type = CardType.Sup;
    }

    public override void Action(BaseChar target)
    {
        base.Action(target);
        target.DecreaseHealth(-healAmount);
    }
}