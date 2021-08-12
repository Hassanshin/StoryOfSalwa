using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackEffect
{
    [SerializeField]
    private List<Buff> buffs = new List<Buff>();
    public List<Buff> Buffs => Buffs;

    public void AddEffect(Buff _buff)
    {
        if (buffs.Contains(_buff))
        {
            Debug.Log("Double effects should replace");
        }

        buffs.Add(_buff);
        // set UI
    }

    public IEnumerator BuffsEffects(BaseChar _char)
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            yield return buffs[i].DoEffect(_char);
            if (buffs[i].lives <= 0)
            {
                // animate buff Set or Hide
                yield return new WaitForSeconds(0.1f);
                buffs[i] = null;
            }
        }

        buffs.RemoveAll((a) => a == null);
    }
}

// Buff list, speedModif, Dps, EvasionModif, AccModif, clearBuff, Stun
// HOOKUP THESE WITH TURN MANAGER AND PLAYER

[System.Serializable]
public class Buff
{
    public string effectName;
    public int lives = 1;

    [Range(-100, 100)]
    public float amount = 20f;

    public virtual IEnumerator DoEffect(BaseChar _char)
    {
        lives--;
        yield return null;
    }

    public virtual IEnumerator ResetEffect(BaseChar _char)
    {
        yield return null;
    }
}

public class SpeedModif : Buff
{
    public override IEnumerator DoEffect(BaseChar _char)
    {
        _char.s_Speed.Add(amount);
        yield return base.DoEffect(_char);
    }

    public override IEnumerator ResetEffect(BaseChar _char)
    {
        _char.s_Speed.Add(-amount);
        return base.ResetEffect(_char);
    }
}