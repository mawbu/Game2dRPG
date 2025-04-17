using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Thêm để sử dụng Slider

public class Boss_Health : MonoBehaviour
{
    public int exReward = 5; // Phần thưởng kinh nghiệm khi đánh bại boss
    public delegate void MonsterDefeated(int exp);
    public static event MonsterDefeated OnMonsterDefeated;

    public int currentHealth;
    public int maxHealth;

    public Slider healthSlider; // Tham chiếu đến thanh máu slider
    public BossZoneManager bossZoneManager; // Tham chiếu đến BossZoneManager
    public Animator anim; // Tham chiếu đến Animator của boss
    private Boss_Movement bossMovement; // Tham chiếu đến Boss_Movement của boss

    private bool isDead = false; // Kiểm tra boss đã chết chưa

    private void Start()
    {
        currentHealth = maxHealth; // Đặt máu khởi đầu bằng maxHealth
        bossMovement = GetComponent<Boss_Movement>(); // Lấy component Boss_Movement

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth; // Đặt giá trị tối đa cho slider
            healthSlider.value = currentHealth; // Đặt giá trị hiện tại cho slider
        }
    }

    public void ChangeHealth(int amount)
    {
        if (isDead) return; // Nếu boss đã chết, không thực hiện
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Giới hạn máu không vượt quá maxHealth
        }
        else if (currentHealth <= 0)
        {
            HandleBossDeath(); // Gọi xử lý logic khi boss chết
        }

        // Cập nhật giá trị slider
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    private void HandleBossDeath()
    {
        isDead = true; // Đánh dấu boss đã chết

        // Ngăn chặn logic di chuyển hoặc trạng thái khác
        if (bossMovement != null)
        {
            bossMovement.enabled = false; // Vô hiệu hóa Boss_Movement
        }

        OnMonsterDefeated?.Invoke(exReward); // Kích hoạt sự kiện thưởng kinh nghiệm

        // Gọi phương thức mở khu vực khi boss bị tiêu diệt
        if (bossZoneManager != null)
        {
            bossZoneManager.BossDefeated(); // Gọi để mở khóa khu vực
        }

        Debug.Log("Boss đã bị đánh bại, khu vực được mở khóa!");

        // Kích hoạt animation Death
        if (anim != null)
        {
            anim.SetTrigger("Death");
            Debug.Log("Triggering Death animation...");
        }

        // Xóa đối tượng sau khi animation Death kết thúc
        StartCoroutine(HandleDestroyAfterDeath());
    }

    private IEnumerator HandleDestroyAfterDeath()
    {
        // Chờ thời gian tương đương với độ dài animation Death (hoặc tùy chỉnh)
        yield return new WaitForSeconds(5.0f); // Điều chỉnh thời gian theo animation
        Destroy(gameObject);
    }
}
