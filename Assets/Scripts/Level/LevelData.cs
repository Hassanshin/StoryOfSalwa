using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Salwa/Level")]

public class LevelData : ScriptableObject
{
    public DialogData dialog;

    [Header("Enemy")]
    public int totalEnemy;

    public CharacterData[] enemies;
    //public CharacterData player;
}