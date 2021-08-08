using System.Collections;
using UnityEngine;

public class EnemyChar : BaseChar
{
    public void Attack()
    {
        if (isDie) { return; }

        BaseChar target = GameManager.Instance.Level.Player;
        CardData curAttack = data.atkCard[Random.Range(0, data.atkCard.Length)];

        // animating
        DoneAttack(target, curAttack);
    }
}