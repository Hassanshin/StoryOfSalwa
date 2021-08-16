using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Salwa/Character")]

public class CharacterData : ScriptableObject
{
    [Header("stats")]
    public float maxHealth = 1000;

    [Range(1, 100)]
    public float speed = 50;
    public float accuracy = 100;
    public float evasion = 0;

    [Header("ATTACK DATA POP")]
    public List<EnemyAtk> atk = new List<EnemyAtk>();

    [Header("sprite")]
    public Sprite icon;
    public RuntimeAnimatorController anim;
    public Color Tint = Color.white;
}

[System.Serializable]
public class EnemyAtk
{
    public CardData card;
    public int coolDown = 1;

    public bool IsReady => coolDown <= 0;

    public EnemyAtk(CardData _card, int _cd)
    {
        card = _card;
        coolDown = _cd;
    }
}