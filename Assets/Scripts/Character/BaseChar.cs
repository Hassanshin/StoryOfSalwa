using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharUI))]
public class BaseChar : MonoBehaviour
{
    [SerializeField]
    private float curHealth = 1000;

    private float maxHealth = 1000;
    public bool IsDie => isDie;
    private bool isDie;

    [SerializeField]
    private float speed = 50f;
    public float Speed => speed;

    [Header("Components")]
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private CharUI ui;

    public virtual void SetData(CharacterData _data)
    {
        maxHealth = _data.maxHealth;
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

        GameManager.Instance.level.GameOverCheck();
    }
}