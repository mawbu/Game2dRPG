using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZoneManager : MonoBehaviour
{
    public GameObject boss; // Đối tượng boss
    public GameObject bossHealthSlider; // Tham chiếu đến thanh máu của boss
    public Collider2D[] mountainColliders; // Colliders ngăn chặn người chơi
    public Collider2D[] boundaryColliders; // Colliders ranh giới
    public elevationExit elevationExit;
    private bool bossDefeated = false; // Theo dõi trạng thái boss

    [SerializeField] private AudioClip bosszone;
    private void Start()
    {
        // Ẩn boss khi khởi động game
        if (boss != null)
        {
            boss.SetActive(false);
        }

        if (bossHealthSlider != null)
        {
            bossHealthSlider.SetActive(false); // Ẩn thanh máu khi khởi đầu
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !bossDefeated)
        {
            LockZone(collision); // Khóa khu vực khi bước vào
            PlayBossRoar();
            ActivateBoss(); // Kích hoạt boss
            ShowBossHealth(); // Hiển thị thanh máu của boss
        }
    }

    void LockZone(Collider2D player)
    {
        foreach (Collider2D mountain in mountainColliders)
        {
            mountain.enabled = false; // Tắt collider núi để khóa lại
        }

        foreach (Collider2D boundary in boundaryColliders)
        {
            boundary.enabled = true; // Bật collider biên giới
        }

        player.GetComponent<SpriteRenderer>().sortingOrder = 30;

        Debug.Log("Khu vực bị khóa! Người chơi không thể thoát ra.");
    }

    void ActivateBoss()
    {
        if (boss != null)
        {
            boss.SetActive(true); // Kích hoạt boss
            Debug.Log("Boss đã xuất hiện!");
        }
    }

    void ShowBossHealth()
    {
        if (bossHealthSlider != null)
        {
            bossHealthSlider.SetActive(true); // Hiển thị thanh máu của boss
            Debug.Log("Thanh máu của boss đã được hiển thị.");
        }
    }

    void PlayBossRoar()
    {
        if (bosszone != null)
        {
            SoundManager.instance.PlaySound(bosszone); // Gọi SoundManager để phát âm thanh
            Debug.Log("Âm thanh của boss đang được phát.");
        }
    }

    public void BossDefeated()
    {
        bossDefeated = true;

        if (elevationExit != null)
        {
            elevationExit.Activate();
        }

        if (bossHealthSlider != null)
        {
            bossHealthSlider.SetActive(false); // Ẩn thanh máu khi boss bị đánh bại
        }

        Debug.Log("Boss đã bị đánh bại, khu vực được mở khóa!");
    }
}
