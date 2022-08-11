using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character States/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")] public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDfence;

    [Header("Kill")] public int killpoint;


    [Header("Level")] public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int curretnExp;
    public float levelBuff;

    public float levelMultiplier
    {
        get { return 1 + (currentLevel - 1) * levelBuff; }
    }

    public void UpdateExp(int point)
    {
        curretnExp += point;
        if (curretnExp >= baseExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        //所有你想提升的数据方法
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
        baseExp += (int) (baseExp * levelMultiplier);
        maxHealth += (int) (maxHealth * levelMultiplier);
    }
}