using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Ult Card", menuName = "Salwa/Ult Card")]

public class CardDataUlt : CardDataAtk
{
    public string clipName = "ult1";

    public CardDataUlt()
    {
        type = CardType.Ult;
    }

    public override void Action(BaseChar target)
    {
        VideoLoader.Instance.PlayDone(clipName, () =>
        {
            base.Action(target);
            TurnManager.Instance.NextTurn();
        });
    }
}