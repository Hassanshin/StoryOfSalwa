using System.Collections;
using UnityEngine;

public class EnemyChar : BaseChar
{
    public void Attack()
    {
        BaseChar target = GameManager.Instance.Level.Player;
        CardData curAttack = data.atkCard[Random.Range(0, data.atkCard.Length)];

        Attacking(target, curAttack);
    }

    public override void Die()
    {
        base.Die();
        GameManager.Instance.Level.WinCheck();
    }
}