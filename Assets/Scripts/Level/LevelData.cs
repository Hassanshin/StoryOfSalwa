using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Salwa/Level")]

public class LevelData : ScriptableObject
{
    [Header("Enemy")]
    public int totalEnemy;

    public GameObject[] enemies;
}