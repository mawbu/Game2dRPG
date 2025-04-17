using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public SkillSlot[] skillSlots;
    public TMP_Text pointsText;
    public int availablePoints;

    private void OnEnable() {
        SkillSlot.OnAbilityPointSpent += HandLeAbilityPointSpent;
        SkillSlot.OnSkillMaxed += HandleSkillMaxed;
        ExpManager.OnLevelUp += UpdateAbilityPoints;
    }

    private void OnDisable() {
        SkillSlot.OnAbilityPointSpent -= HandLeAbilityPointSpent;
        SkillSlot.OnSkillMaxed -= HandleSkillMaxed;
        ExpManager.OnLevelUp -= UpdateAbilityPoints;
    }

    private void Start() {
        foreach (SkillSlot slot in skillSlots)
        {
            slot.skillButton.onClick.AddListener(() => CheckAvailablePoints(slot));
        }
        UpdateAbilityPoints(0);

        // Khi bắt đầu trò chơi, tải lại thông tin từ PlayerPrefs
        LoadSkillData();
    }

    private void CheckAvailablePoints(SkillSlot slot)
    {
        if(availablePoints > 0)
        {
            slot.TryUpgradeSkill();
        }
    }

    private void HandLeAbilityPointSpent(SkillSlot skillSlot)
    {
        if(availablePoints > 0)
        {
            UpdateAbilityPoints(-1);
        }
        if (skillSlot.skillSO.skillSound != null)
        {
            SoundManager.instance.PlaySound(skillSlot.skillSO.skillSound);
        }
        SaveSkillData();  // Lưu lại dữ liệu sau khi chi tiêu điểm kỹ năng
    }

    private void HandleSkillMaxed(SkillSlot skillSlot)
    {
        foreach(SkillSlot slot in skillSlots)
        {
            if(!slot.isUnlocked && slot.CanUnlockSkill())
            {
                slot.UnLock();
            }
        }
    }

    public void UpdateAbilityPoints(int amount)
    {
        availablePoints += amount;
        pointsText.text = "Points " + availablePoints;
        SaveSkillData();  // Lưu lại dữ liệu mỗi khi thay đổi điểm
    }

    // Lưu lại các kỹ năng và điểm kỹ năng vào PlayerPrefs
    private void SaveSkillData()
    {
        // Lưu số điểm kỹ năng
        PlayerPrefs.SetInt("AvailablePoints", availablePoints);

        // Lưu trạng thái của mỗi skill
        for (int i = 0; i < skillSlots.Length; i++)
        {
            PlayerPrefs.SetInt("SkillUnlocked" + i, skillSlots[i].isUnlocked ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    // Tải lại các kỹ năng và điểm kỹ năng từ PlayerPrefs
    private void LoadSkillData()
    {
        // Tải lại điểm kỹ năng
        if (PlayerPrefs.HasKey("AvailablePoints"))
        {
            availablePoints = PlayerPrefs.GetInt("AvailablePoints");
            pointsText.text = "Points " + availablePoints;
        }

        // Tải lại trạng thái của các kỹ năng
        for (int i = 0; i < skillSlots.Length; i++)
        {
            if (PlayerPrefs.HasKey("SkillUnlocked" + i))
            {
                bool isUnlocked = PlayerPrefs.GetInt("SkillUnlocked" + i) == 1;
                skillSlots[i].isUnlocked = isUnlocked;
            }
        }
    }
}

