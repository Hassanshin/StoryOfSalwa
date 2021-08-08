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

    [SerializeField]
    protected float speed = 50f;
    public float Speed => speed;

    [Header("Components")]
    [SerializeField]
    protected Animator anim;

    [SerializeField]
    protected CharUI ui;

    [SerializeField]
    protected CharacterData data;

    // move to set data
    public void Initialized()
    {
        ui = GetComponent<CharUI>();
    }

    public virtual void SetData(CharacterData _data)
    {
        data = _data;

        maxHealth = data.maxHealth;
        curHealth = maxHealth;

        ui.SetHealth(curHealth / maxHealth);
        isDie = false;
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

        //TurnManager.Instance.RemoveTurn(this);
        GameManager.Instance.Level.GameOverCheck();
    }

    public virtual void DoneAttack(BaseChar target, CardData cardData)
    {
        // trigger animation?
        cardData.Action(target);
        TurnManager.Instance.NextTurn();
    }
}