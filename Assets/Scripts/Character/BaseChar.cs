using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharUI))]
public class BaseChar : MonoBehaviour
{
    [SerializeField]
    protected float curHealth = 1000;

    protected float maxHealth = 1000;
    public bool IsDie => isDie;
    protected bool isDie;

    protected float speed;
    public float Speed => speed;

    [Header("Components")]
    [SerializeField]
    protected Animator anim;

    [SerializeField]
    protected CharUI ui;

    [SerializeField]
    protected CharacterData data;

    public virtual void SetData(CharacterData _data)
    {
        //ui = GetComponent<CharUI>();
        //anim = transform.GetChild(0).GetChild(0).GetComponent<Animator>();

        data = _data;

        maxHealth = data.maxHealth;
        curHealth = maxHealth;

        ui.SetHealth(curHealth / maxHealth);
        isDie = false;

        speed = data.speed;
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

        TurnManager.Instance.ui.FindCharUi(this).gameObject.SetActive(false);
        //TurnManager.Instance.RemoveTurn(this);
    }

    public virtual void DoneAttack(BaseChar target, CardData cardData)
    {
        // trigger animation?
        cardData.Action(target);
        TurnManager.Instance.NextTurn();
    }
}