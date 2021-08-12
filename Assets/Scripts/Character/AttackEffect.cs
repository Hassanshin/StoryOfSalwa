using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackEffect
{
    [SerializeField]
    private List<Buff> buffs = new List<Buff>();
    public List<Buff> Buffs => Buffs;

    private BaseChar user;

    public void SetCharcter(BaseChar _char)
    {
        user = _char;
    }

    public IEnumerator AddEffect(Buff buff)
    {
        // animate buff

        if (buffs.Contains(buff))
        {
            Debug.Log("Double effects should replace");
        }

        buffs.Add(buff);

        yield return buff.DoEffect(user);
        // set UI
    }

    public IEnumerator TurnPassed()
    {
        List<Buff> removedBuff = new List<Buff>();
        foreach (Buff a in buffs)
        {
            a.mLives--;

            if (a.mLives <= 0)
            {
                // animate buff
                yield return new WaitForSeconds(0.1f);
                yield return a.ResetEffect(user);
                removedBuff.Add(a);
            }
            else
            {
                yield return a.DoEffect(user);
            }
        }

        buffs.RemoveAll(l => removedBuff.Contains(l));
    }
}

// Buff list, speedModif, Dps, EvasionModif, AccModif, clearBuff, Stun
// HOOKUP THESE WITH TURN MANAGER AND PLAYER

[System.Serializable]
public class Buff
{
    public string mName;
    public int mLives = 1;

    [Range(-100, 100)]
    public float mAmount = 20f;

    public virtual IEnumerator DoEffect(BaseChar _char)
    {
        yield return null;
    }

    public virtual IEnumerator ResetEffect(BaseChar _char)
    {
        yield return null;
    }

    public Buff(string name, int lives, float amount)
    {
        mName = name;
        mLives = lives;
        mAmount = amount;
    }
}

public class SpeedModif : Buff
{
    public SpeedModif(string name, int lives, float amount)
        : base(name, lives, amount)
    {
        mName = name;
        mLives = lives;
        mAmount = amount;
    }

    public override IEnumerator DoEffect(BaseChar _char)
    {
        _char.s_Speed.Add(mAmount);
        yield return base.DoEffect(_char);
    }

    public override IEnumerator ResetEffect(BaseChar _char)
    {
        _char.s_Speed.Add(-mAmount);
        return base.ResetEffect(_char);
    }
}