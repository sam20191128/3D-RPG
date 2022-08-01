using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO temlateData;
    public CharacterData_SO characterData;
    public AttackData_SO attackData;

    [HideInInspector] public bool isCritical;

    private void Awake()
    {
        if (temlateData != null)
        {
            characterData = Instantiate(temlateData);
        }
    }


    #region Read from Data_SO

    public int MaxHealth
    {
        get
        {
            if (characterData != null) return characterData.maxHealth;
            else return 0;
        }
        set { characterData.maxHealth = value; }
    }

    public int CurrentHealth
    {
        get
        {
            if (characterData != null) return characterData.currentHealth;
            else return 0;
        }
        set { characterData.currentHealth = value; }
    }

    public int BaseDefence
    {
        get
        {
            if (characterData != null) return characterData.baseDefence;
            else return 0;
        }
        set { characterData.baseDefence = value; }
    }

    public int CurrentDfence
    {
        get
        {
            if (characterData != null) return characterData.currentDfence;
            else return 0;
        }
        set { characterData.currentDfence = value; }
    }

    #endregion

    #region Character Combat

    public void TakeDamge(CharacterStats attacker, CharacterStats defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.CurrentDfence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (attacker.isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }
        //TODO Update UI
        //TODO 经验update
    }

    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamge, attackData.maxDamge);

        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
            Debug.Log("暴击！" + coreDamage);
        }

        return (int) coreDamage;
    }

    #endregion
}