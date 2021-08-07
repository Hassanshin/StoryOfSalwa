using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ult Card", menuName = "Salwa/Ult Card")]

public class CardDataUlt : CardDataAtk
{
    public UnityEngine.Video.VideoClip clipName;

    public CardDataUlt()
    {
        type = CardType.Ult;
    }

    public override void Action(BaseChar target)
    {
        base.Action(target);
        Debug.Log($"Playing {clipName}");
    }
}