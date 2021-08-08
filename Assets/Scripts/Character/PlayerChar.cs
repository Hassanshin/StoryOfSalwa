using System.Collections;
using UnityEngine;

public class PlayerChar : BaseChar
{
    public override void DoneAttack(BaseChar target, CardData cardData)
    {
        // animating
        base.DoneAttack(target, cardData);
    }
}