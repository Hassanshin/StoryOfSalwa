using System.Collections;
using UnityEngine;

[System.Serializable]
public class BuffCardData
{
    [Header("Instant Effect", order = 2)]
    public bool stun = false;
    public bool cure = false;

    [Header("Buff Stats", order = 2)]
    [Range(-30, 30)] public float speedAmount = 0;
    [Range(-30, 30)] public float accuracyAmount = 0;
    [Range(-30, 30)] public float evasionAmount = 0;
    public float damagePerSecondAmount = 0;

    [Header("Buff Duration", order = 2)]
    public int buffDuration = 1;
}