using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseChar : MonoBehaviour
{
    [SerializeField]
    private float health = 1000;

    [SerializeField]
    private Animator anim;

    public virtual void SetData(CharacterData _data)
    {
        health = _data.health;
    }

    public virtual void DecreaseHealth(float _amount)
    {
        health -= _amount;
        health = Mathf.Clamp(health, 0, Mathf.Infinity);

        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
    }
}