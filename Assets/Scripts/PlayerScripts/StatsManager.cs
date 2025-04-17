using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum PlayerState
{
    Sword,
    Bow
}

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;
    public TMP_Text healthText;

    [Header("Combat Stats")]
    public int sworddamage;
    public int bowdamage;
    public float weaponRange;
    public float knockbackForce;
    public float knockbackTime;
    public float stunTime;

    [Header("Movement Stats")]
    public int swordspeed;
    public int bowspeed;

    [Header("Health Stats")]
    public int maxHealth;
    public int currentHealth;
////////////////////////////
    public PlayerState currentState; // Trạng thái hiện tại

    private void Awake() {
        if(Instance == null)
           Instance = this;
           else
                Destroy(gameObject);
    }

    public void UpdateStats(PlayerState state)
    {
        currentState = state;

        // Gọi StatsUI để cập nhật
        StatsUI.Instance.UpdateAllStats();
    }

    public void UpdateMaxHealth(int amount)
    {
        maxHealth += amount;
        healthText.text = "HP: " + currentHealth + "/ " + maxHealth;
    }


    public void UpdateDamge(int amount)
    {
        sworddamage += amount;
        bowdamage += amount;
        StatsUI.Instance.UpdateDamage();
    }

    public void HealPlayer(int amount) {
    if (currentHealth >= maxHealth) {
        // Nếu currentHealth đã đạt maxHealth, không hồi máu thêm
        return;
    }
    
    // Tăng sức khỏe nhưng không vượt quá maxHealth
    currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    healthText.text = "HP: " + currentHealth + "/ " + maxHealth;
}

    // public void ResetPlayerStats()
    // {
    //     // Khôi phục HP và damage từ PlayerPrefs
    //     if (PlayerPrefs.HasKey("PlayerMaxHealth"))
    //     {
    //         maxHealth = PlayerPrefs.GetInt("PlayerMaxHealth");
    //     }
    //     if (PlayerPrefs.HasKey("PlayerSwordDamage"))
    //     {
    //         sworddamage = PlayerPrefs.GetInt("PlayerSwordDamage");
    //     }
    //     if (PlayerPrefs.HasKey("PlayerBowDamage"))
    //     {
    //         bowdamage = PlayerPrefs.GetInt("PlayerBowDamage");
    //     }

    //     // Khôi phục lại sức khỏe hiện tại và cập nhật UI
    //     currentHealth = maxHealth;
    //     healthText.text = "HP: " + currentHealth + "/ " + maxHealth;

    //     // Cập nhật UI với damage
    //     StatsUI.Instance.UpdateDamage();
    // }

}

