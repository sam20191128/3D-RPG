using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    private Text levelTex;
    private Image healthSlider;
    private Image expSlider;

    private void Awake()
    {
        levelTex = transform.GetChild(0).GetComponent<Text>();
        healthSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(2).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        levelTex.text = "Level " + GameManager.Instance.playerStats.characterData.currentLevel.ToString("00");
        UpdateHealth();
        UpdateExp();
    }

    private void UpdateHealth()
    {
        float sliderPercent = (float) GameManager.Instance.playerStats.CurrentHealth / GameManager.Instance.playerStats.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    private void UpdateExp()
    {
        float sliderPercent = (float) GameManager.Instance.playerStats.characterData.curretnExp / GameManager.Instance.playerStats.characterData.baseExp;
        expSlider.fillAmount = sliderPercent;
    }
}