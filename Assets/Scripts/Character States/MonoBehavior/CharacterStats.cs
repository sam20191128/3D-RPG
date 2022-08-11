using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack;
    public CharacterData_SO temlateData;
    public CharacterData_SO characterData;
    public AttackData_SO attackData;

    [HideInInspector] public bool isCritical;

    public GameObject deathEffect;

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

    public void TakeDamage(CharacterStats attacker, CharacterStats defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.CurrentDfence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (attacker.isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }

        if (deathEffect != null)
        {
            GameObject DeathEffect = Instantiate(deathEffect, defener.transform.position + new Vector3(0, 0.5f, 0), Quaternion.LookRotation(attacker.gameObject.transform.forward));
            Destroy(DeathEffect, 3f);
        }

        //TODO Update UI
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);

        //TODO 经验update
        if (CurrentHealth <= 0)
        {
            attacker.characterData.UpdateExp(characterData.killpoint);
        }
    }

    public void TakeDamage(int damage, CharacterStats defener)
    {
        int currentDamage = Mathf.Max(damage - defener.CurrentDfence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamage, 0);
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        GameManager.Instance.playerStats.characterData.UpdateExp(characterData.killpoint);
    }

    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);

        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
            Debug.Log("暴击！" + coreDamage);
        }

        return (int) coreDamage;
    }

    #endregion
}