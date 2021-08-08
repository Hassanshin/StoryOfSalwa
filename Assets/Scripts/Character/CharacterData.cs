using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Salwa/Character")]

public class CharacterData : ScriptableObject
{
    public float maxHealth;
    public CardData[] atkCard;
}