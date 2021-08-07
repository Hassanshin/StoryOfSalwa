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

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private CharUI ui;

    public virtual void SetData(CharacterData _data)
    {
        maxHealth = _data.maxHealth;
        curHealth = maxHealth;

        ui.SetHealth(curHealth / maxHealth);
    }

    public virtual void DecreaseHealth(float _amount)
    {
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
    }
}