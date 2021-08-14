using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackEffect
{
    [SerializeField]
    private List<Buff> buffs = new List<Buff>();
    public List<Buff> Buffs => Buffs;
    private List<Buff> removedBuff = new List<Buff>();

    private BaseChar user;

    public void SetCharcter(BaseChar _char)
    {
        user = _char;
    }

    public IEnumerator AddEffect(Buff buff)
    {
        // animate buff

        if (isDoubleBuff(buff))
        {
            //Debug.Log("Double effects should replace");
            Buff doubledBuff = buffs.Find((a) => a.mName == buff.mName);

            doubledBuff.mLives = buff.mLives;
        }
        else
        {
            buffs.Add(buff);
            if (!TurnManager.Instance.isCurrentPlaying(user))
            {
                yield return buff.StartEffect(user);
            }
        }
    }

    private bool isDoubleBuff(Buff buff)
    {
        foreach (Buff item in buffs)
        {
            if (item.mName == buff.mName)
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerator PreTurnEffect()
    {
        foreach (Buff a in buffs)
        {
            a.mLives--;

            yield return new WaitForSeconds(0.1f);
            yield return a.PreTurnEffect(user);
        }
    }

    public IEnumerator PostTurnEffect()
    {
        foreach (Buff a in buffs)
        {
            if (!a.mStartEffect)
            {
                yield return new WaitForSeconds(0.1f);
                yield return a.StartEffect(user);
            }

            if (a.mLives <= 0)
            {
                yield return new WaitForSeconds(0.1f);
                yield return a.FinishEffect(user);
                removedBuff.Add(a);
            }
            else
            {
                yield return a.PostTurnEffect(user);
            }
        }

        buffs.RemoveAll(l => removedBuff.Contains(l));
        removedBuff.Clear();
    }
}

// Buff list, speedModif, Dps, EvasionModif, AccModif, clearBuff, Stun
// HOOKUP THESE WITH TURN MANAGER AND PLAYER

[System.Serializable]
public class Buff
{
    public string mName;
    public int mLives = 1;
    public BuffType mType;

    [Range(-100, 100)]
    public float mAmount = 20f;

    public bool mStartEffect;

    public Buff(string name, BuffType type, int lives, float amount)
    {
        mName = name;
        mType = type;
        mLives = lives;
        mAmount = amount;
    }

    public IEnumerator StartEffect(BaseChar user)
    {
        switch (mType)
        {
            case BuffType.speed:
                user.s_Speed.Add(mAmount);
                mStartEffect = true;

                yield return TurnManager.Instance.SpeedEffectBuff(user, mAmount);
                Debug.Log("Start effect");

                break;
        }
        yield return null;
    }

    public IEnumerator PreTurnEffect(BaseChar user)
    {
        switch (mType)
        {
            case BuffType.speed:

                break;
        }
        yield return null;
    }

    public IEnumerator PostTurnEffect(BaseChar user)
    {
        switch (mType)
        {
            case BuffType.speed:

                break;
        }
        yield return null;
    }

    public IEnumerator FinishEffect(BaseChar user)
    {
        switch (mType)
        {
            case BuffType.speed:
                user.s_Speed.Add(-mAmount);
                yield return TurnManager.Instance.SpeedEffectBuff(user, -mAmount);
                Debug.Log("Finish effect");

                break;
        }
        yield return null;
    }
}

public enum BuffType { speed, dps, eva, acc, clear, stun }