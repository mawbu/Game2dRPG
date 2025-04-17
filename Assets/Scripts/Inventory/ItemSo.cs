using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class ItemSo : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public int amountToChangeStat;
    public AttributesToChange attributesToChange = new AttributesToChange();
    public int amountToChangAttrinute;

    public bool UseItem()
    {
        if(statToChange == StatToChange.health)
        {
            StatsManager statsManager = GameObject.Find("StatsManager").GetComponent<StatsManager>();
            if(statsManager.currentHealth == statsManager.maxHealth)
            {
                return false;
            }
            else
            {
                statsManager.HealPlayer(amountToChangeStat);
                return true;
            }
        }
        return false;
    }



    public enum StatToChange
    {
        none,
        health,
        mana,
        stamina
    }

    public enum AttributesToChange
    {
        none,
        strength,
        defense,
        intelligence,
        agility
    }
}
