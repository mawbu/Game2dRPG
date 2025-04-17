using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpManager : MonoBehaviour
{
    public static ExpManager instance;
    public int level;
    public int currentExp;
    public int expToLevel = 10;
    public float expGrowthMultiplier = 1.2f;  // Add 20% more EXP to level each time
    public Slider expSlider;
    public TMP_Text currentLevelText;
    public static event Action<int> OnLevelUp;


    private void Start() {
        //LoadPlayerData();
        UpdateUI();
    }

   private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Return))
        // {
        //     GainExperience(2);
        // }
    }

    private void OnEnable() {
        Enemy_Health.OnMonsterDefeated += GainExperience;
        Boss_Health.OnMonsterDefeated += GainExperience;
    }

    private void OnDisable() {
        Enemy_Health.OnMonsterDefeated -= GainExperience;
        Boss_Health.OnMonsterDefeated -= GainExperience;
    }

    

    public void GainExperience(int amount)
    {
        currentExp += amount;
        if(currentExp >= expToLevel)
        {
            LevelUp();
        }
        UpdateUI();
        //SavePlayerData();
    }

    private void LevelUp()
    {
        level++;
        currentExp -= expToLevel;
        expToLevel = Mathf.RoundToInt(expToLevel * expGrowthMultiplier);
        OnLevelUp?.Invoke(1);
        //SavePlayerData();
    }


    public void UpdateUI()
    {
        expSlider.maxValue = expToLevel;
        expSlider.value = currentExp;
        currentLevelText.text = "Level: " + level;
    }

    // Lưu lại thông tin của người chơi
    // private void SavePlayerData()
    // {
    //     PlayerPrefs.SetInt("PlayerLevel", level);
    //     PlayerPrefs.SetInt("PlayerCurrentExp", currentExp);
    //     PlayerPrefs.SetInt("PlayerExpToLevel", expToLevel);
    //     PlayerPrefs.Save();
    // }

    // // Tải lại thông tin của người chơi
    // public void LoadPlayerData()
    // {
    //     if (PlayerPrefs.HasKey("PlayerLevel"))
    //     {
    //         level = PlayerPrefs.GetInt("PlayerLevel");
    //         currentExp = PlayerPrefs.GetInt("PlayerCurrentExp");
    //         expToLevel = PlayerPrefs.GetInt("PlayerExpToLevel");
    //     }
    // }
}
