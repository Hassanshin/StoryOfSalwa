using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharUI))]
public class BaseChar : MonoBehaviour
{
    [SerializeField]
    protected float curHealth = 1000;

    protected float maxHealth = 1000;
    public bool IsDie => isDie;
    protected bool isDie;

    public Stats s_Speed;

    public Stats s_Acc;

    public Stats s_Eva;

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

    public IEnumerator BuffActive()
    {
        yield return effects.TurnPassed();
    }

    internal void AddBuff(Buff buff)
    {
        StartCoroutine(effects.AddEffect(buff));
    }

    public virtual void SetData(CharacterData _data)
    {
        //ui = GetComponent<CharUI>();
        //anim = transform.GetChild(0).GetChild(0).GetComponent<Animator>();

        data = _data;
        effects.SetCharcter(this);

        maxHealth = data.maxHealth;
        curHealth = maxHealth;

        ui.SetHealth(curHealth / maxHealth);
        isDie = false;

        s_Speed.Set(data.speed);
        s_Acc.Set(data.accuracy);
        s_Eva.Set(data.evasion);

        anim.runtimeAnimatorController = _data.anim;
    }

    public virtual void DecreaseHealth(float _amount)
    {
        if (isDie) { return; }
        curHealth -= _amount;
        curHealth = Mathf.Clamp(curHealth, 0, maxHealth);

        ui.SetHealth(curHealth / maxHealth);
        ui.SetFloatingText(-_amount);

        if (curHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
        isDie = true;

        AudioManager.Instance.PlaySfx(4);

        TurnManager.Instance.ui.FindCharUi(this).gameObject.SetActive(false);
        //TurnManager.Instance.RemoveTurn(this);
    }

    public virtual void Attacking(BaseChar target, CardData cardData)
    {
        if (cardData.type == CardType.Ult)
        {
            cardData.Action(target);
        }
        else
        {
            anim.Play("atk");
            DoCardMove.AddListener(() =>
            {
                cardData.Action(target);
            });
        }
    }

    // called on idle start
    public virtual void FinishedAnimating()
    {
        DoCardMove?.Invoke();

        DoCardMove.RemoveAllListeners();
    }
}

[System.Serializable]
public class Stats
{
    private float value;
    public float Value => value;

    public void Set(float a)
    {
        value = a;
    }

    public void Add(float a)
    {
        value += a;
    }
}