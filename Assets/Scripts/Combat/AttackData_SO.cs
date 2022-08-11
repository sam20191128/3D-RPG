using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;
    public float skillRange;
    public float coolDown;
    [FormerlySerializedAs("minDamge")] public int minDamage;
    [FormerlySerializedAs("maxDamge")] public int maxDamage;
    public float criticalMultiplier;
    public float criticalChance;
}