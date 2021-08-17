using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharUI))]
public class BaseChar : MonoBehaviour
{
    [SerializeField]
    protected ElementType elemType;
    public ElementType ElemType => elemType;
    public bool IsDie => isDie;

    protected bool isDie;

    public Stats s_Health;

    public Stats s_Speed;

    public Stats s_Acc;

    public Stats s_Eva;

    public bool IsStunned => isStunned;
    protected bool isStunned;

    [Header("Components")]
    [SerializeField]
    protected Animator anim;

    [SerializeField]
    protected CharUI ui;

    public CharacterData CharData => data;

    [SerializeField]
    protected CharacterData data;

    public UnityEvent DoCardMove;

    [Header("Components")]
    [SerializeField]
    private AttackEffect effects;

    #region Buff

    public IEnumerator PreTurnBuff()
    {
        yield return effects.PreTurnEffect();
    }

    public IEnumerator PostTurnBuff()
    {
        yield return effects.PostTurnEffect();
    }

    public void AddBuff(Buff buff)
    {
        StartCoroutine(effects.AddEffect(buff));
    }

    public void SetStun(bool state = true)
    {
        isStunned = state;
    }

    public void DispelEffect()
    {
        effects.Dispel();
    }

    #endregion Buff

    public virtual void SetData(CharacterData _data)
    {
        data = _data;

        gameObject.name = data.name;
        effects.SetCharcter(this);

        elemType = data.elemType;

        s_Health.Set(data.maxHealth, true);

        ui.SetHealth(s_Health.CurValue / s_Health.DefaultValue);
        isDie = false;

        s_Speed.Set(data.speed, true);
        s_Acc.Set(data.accuracy, true);
        s_Eva.Set(data.evasion, true);

        anim.runtimeAnimatorController = data.anim;
        anim.GetComponent<SpriteRenderer>().color = data.Tint;
    }

    public virtual void IncreaseHealth(float _amount)
    {
        if (isDie || _amount == 0) { return; }
        s_Health.Add(_amount);
        s_Health.Set(Mathf.Clamp(s_Health.CurValue, 0, s_Health.DefaultValue));

        ui.SetHealth(s_Health.CurValue / s_Health.DefaultValue);
        ui.SetFloatingText(_amount);

        if (s_Health.CurValue <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
        isDie = true;

        AudioManager.Instance.PlaySfx(4);

        TurnManager.Instance.FindCharUi(this).gameObject.SetActive(false);
        //TurnManager.Instance.RemoveTurn(this);
    }

    public virtual void TurnPhase()
    {
    }

    public virtual void Attacking(BaseChar target, CardData cardData)
    {
        bool willHit = target == this ? true : calculateHitAccuracy(target);

        if (cardData.type == CardType.Ult)
        {
            hit(target, cardData, willHit);
        }
        else
        {
            anim.Play("atk");
            DoCardMove.AddListener(() =>
            {
                hit(target, cardData, willHit);
            });
        }
    }

    private void hit(BaseChar target, CardData cardData, bool willHit)
    {
        if (!willHit)
        {
            ui.SetFloatingText("Miss");
            target.ui.SetFloatingText("Evade");
            return;
        }

        cardData.Action(target);
    }

    private bool calculateHitAccuracy(BaseChar target)
    {
        float chance = s_Acc.CurValue * (100 - target.s_Eva.CurValue) / 100;
        bool isHit = Random.Range(0f, 100f) <= chance;
        return isHit;
    }

    // called on empty start animation
    public virtual void FinishedAnimating()
    {
        DoCardMove?.Invoke();

        DoCardMove.RemoveAllListeners();
    }
}

[System.Serializable]
public class Stats
{
    [SerializeField]
    private float value;
    public float CurValue => value;

    private float defaultValue;
    public float DefaultValue => defaultValue;
    public float DeltaValue => value - defaultValue;

    public void Set(float a, bool isDefault = false)
    {
        if (isDefault)
        {
            defaultValue = a;
        }
        value = a;
    }

    public void Add(float a)
    {
        value += a;
    }

    public void Reset()
    {
        value = defaultValue;
    }
}