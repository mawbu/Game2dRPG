using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTNT_Combat : MonoBehaviour
{
    // public int damage = 1;
    public Transform attackPoint; // Vị trí tấn công của kẻ địch
    public float weaponRange = 7f; // Phạm vi phát hiện
    public float projectileSpeed = 5f; // Tốc độ đạn
    public float attackCooldown = 2f; // Thời gian hồi chiêu
    public LayerMask playerLayer;
    public GameObject projectilePrefab; // Prefab đạn
    private float attackCooldownTimer = 0f;

    private bool isAttacking = false; // Trạng thái đang tấn công
    private Animator animator; // Bộ điều khiển hoạt ảnh

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        attackCooldownTimer -= Time.deltaTime;

        // Nếu không trong trạng thái tấn công và cooldown đã xong, kiểm tra player
        if (!isAttacking && attackCooldownTimer <= 0)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);
            if (hits.Length > 0)
            {
                // Bắt đầu hoạt ảnh tấn công
                isAttacking = true;
                attackCooldownTimer = attackCooldown; // Reset cooldown
                animator.SetBool("isAttacking", true); // Kích hoạt hoạt ảnh tấn công
            }
        }
    }

    public void ShootProjectile()
    {
        // Kích hoạt từ Animation Event
        if (projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            TNT tnt = projectile.GetComponent<TNT>();

            if (rb != null && tnt != null)
            {
                Vector2 playerPosition = FindPlayerPosition();
                Vector2 direction = (playerPosition - (Vector2)attackPoint.position).normalized;
                tnt.direction = direction; // Gửi hướng cho TNT script
                rb.linearVelocity = direction * projectileSpeed; // Cập nhật vận tốc đạn theo hướng
            }
        }
    }

    private Vector2 FindPlayerPosition()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);
        if (hits.Length > 0)
        {
            return hits[0].transform.position; // Trả về vị trí người chơi
        }
        return attackPoint.position; // Nếu không tìm thấy, trả về vị trí hiện tại
    }

    public void FinishAttack()
    {
        // Kích hoạt từ Animation Event để kết thúc trạng thái tấn công
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        // Hiển thị phạm vi phát hiện trong chế độ phát triển
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
    }
}
