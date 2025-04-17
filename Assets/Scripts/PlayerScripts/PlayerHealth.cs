using System.Collections;
using UnityEngine;
using TMPro;
using Cinemachine;

public class PlayerHealth : MonoBehaviour
{
    public TMP_Text healthText;
    public Animator healthTextAnim;
    private Animator anim;

    private void Start() {
        healthText.text = "HP: " + StatsManager.Instance.currentHealth + " / " + StatsManager.Instance.maxHealth;
        anim = GetComponent<Animator>(); // Lấy Animator của nhân vật
    }

    public void ChangeHealth(int amount)
    {
        StatsManager.Instance.currentHealth += amount;
        healthTextAnim.Play("TextUpdate");
        healthText.text = "HP: " + StatsManager.Instance.currentHealth + " / " + StatsManager.Instance.maxHealth;

        if(StatsManager.Instance.currentHealth <= 0)
        {
            TriggerDeath();
            GameEnd.instance.OpenEndScreen(); // Gọi hàm TriggerDeath khi HP <= 0
        }
    }

    private void TriggerDeath()
    {
        if (anim == null) {
            
            return;
        }

        bool hasDeathTrigger = false;
        foreach (var param in anim.parameters)
        {
            if (param.name == "Death" && param.type == AnimatorControllerParameterType.Trigger)
            {
                hasDeathTrigger = true;
                break;
            }
        }

        if (!hasDeathTrigger)
        {
           
            return;
        }

        
        anim.SetTrigger("Death"); // Kích hoạt trigger "Death"
        
    }

    // private IEnumerator RespawnPlayer()
    // {
    //     yield return new WaitForSeconds(2f); // Tạm dừng 2 giây trước khi hồi sinh

    //     // Reset lại trạng thái Animator để nhân vật không ở trạng thái "Death" nữa
    //     anim.ResetTrigger("Death");
    //     anim.Play("idle"); // Đặt lại trạng thái về "Idle" hoặc trạng thái mặc định của nhân vật

    //     // Đặt lại HP và vị trí của nhân vật
    //     StatsManager.Instance.currentHealth = StatsManager.Instance.maxHealth; // Đặt lại HP về max
    //     healthText.text = "HP: " + StatsManager.Instance.currentHealth + " / " + StatsManager.Instance.maxHealth;
    //     transform.position = new Vector3(-4.89f, 0.66f, 0); // Đặt lại vị trí bắt đầu của nhân vật

    //     // Đảm bảo rằng camera theo dõi lại Player sau khi hồi sinh
    //     CinemachineVirtualCamera vCam = FindObjectOfType<CinemachineVirtualCamera>();
    //     if (vCam != null)
    //     {
    //         vCam.Follow = transform; // Đặt lại Follow cho camera là transform của Player
    //     }

    //     gameObject.SetActive(true); // Kích hoạt lại nhân vật
    // }
}
