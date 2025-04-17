using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillManager : MonoBehaviour
{
  
    private void OnEnable() {
        SkillSlot.OnAbilityPointSpent += HandleAbilityPointSpent;
    }

    private void OnDisable() {
        SkillSlot.OnAbilityPointSpent -= HandleAbilityPointSpent;
    }

    private void HandleAbilityPointSpent(SkillSlot slot)
    {
        string skillName = slot.skillSO.skillName;
        switch(skillName)
        {
            case "Max Health Boost":
                StatsManager.Instance.UpdateMaxHealth(1);
                break;
            case "Sword Slash":
                StatsManager.Instance.UpdateDamge(1);
                break;

            default: 
                Debug.LogWarning("Unknow skill: " + skillName);
                break;
        }
        
    }

}
