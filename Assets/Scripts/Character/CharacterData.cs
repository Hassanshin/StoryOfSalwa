using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Salwa/Character")]

public class CharacterData : ScriptableObject
{
    [Header("stats")]
    public float maxHealth = 1000;

    [Range(1, 100)]
    public float speed = 50;
    public CardData[] atkCard;

    [Header("sprite")]
    public RuntimeAnimatorController anim;
}