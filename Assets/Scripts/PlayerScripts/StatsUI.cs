using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    public static StatsUI Instance;
    public GameObject[] statsSlots;
    public CanvasGroup statsCanvas;
    private bool statsOpen = false;


    private void Awake()
    {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
    }
    private void Start() {
        UpdateAllStats();
    }

    private void Update() 
    {
        if(Input.GetButtonDown("ToggleStats"))
            if(statsOpen)
                {
                    //Time.timeScale = 1;
                    // UpdateAllStats();
                    statsCanvas.alpha = 0;
                    statsOpen = false;
                }
            else
                {                  
                    //Time.timeScale = 0;
                    // UpdateAllStats();
                    statsCanvas.alpha = 1;
                    statsOpen = true;
                }
    }

    public void UpdateDamage()
    {
        if (StatsManager.Instance.currentState == PlayerState.Sword)
        {
            statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Damage: " + StatsManager.Instance.sworddamage;
        }
        else if (StatsManager.Instance.currentState == PlayerState.Bow)
        {
            statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Damage: " + StatsManager.Instance.bowdamage;
        }
    }

    public void UpdateSpeed()
    {
        if (StatsManager.Instance.currentState == PlayerState.Sword)
        {
            statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Speed: " + StatsManager.Instance.swordspeed;
        }
        else if (StatsManager.Instance.currentState == PlayerState.Bow)
        {
            statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Speed: " + StatsManager.Instance.bowspeed;
        }
    }


    public void UpdateAllStats()
    {
        UpdateDamage();
        UpdateSpeed();
    }
}
