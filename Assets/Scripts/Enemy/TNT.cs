using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 direction = Vector2.right;
    public float lifeSpawn = 4f;
    public float speed;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    public SpriteRenderer sr;
    public Sprite buriedSprite;
    public GameObject explosionEffectPrefab; // Prefab hiệu ứng nổ
    public float explosionRadius = 2f; // Bán kính vụ nổ
    public int explosionDamage = 2; // Sát thương vụ nổ
    public float knockbackForce = 5f; // Lực knockback
    public float stunTime = 0.5f; // Thời gian choáng
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip fuseSound;

    void Start()
    {
        rb.linearVelocity = direction * speed; // Di chuyển TNT
        RotateArrow();
        if (fuseSound != null)
        {
            SoundManager.instance.PlaySound(fuseSound, 0.7f);
        }
        Invoke(nameof(Explode), lifeSpawn); // Tự nổ sau thời gian sống
    }

    private void RotateArrow()
    {
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Nếu TNT chạm người chơi
        if ((playerLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-explosionDamage); // Gây sát thương
            }

            var playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.Knockback(transform, knockbackForce, stunTime); // Áp dụng knockback
            }

            Explode(); // Kích hoạt nổ
        }
        // Nếu TNT chạm vật thể
        else if ((obstacleLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            Explode(); // Kích hoạt nổ
        }
    }

    private void Explode()
    {
        // Phát âm thanh "nổ"
        if (explosionSound != null)
        {
            SoundManager.instance.PlaySound(explosionSound, 1.0f);
        }
        // Hiển thị hiệu ứng nổ
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Gây sát thương và knockback cho tất cả đối tượng trong bán kính vụ nổ
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hit in hits)
        {
            if ((playerLayer.value & (1 << hit.gameObject.layer)) > 0)
            {
                var playerHealth = hit.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.ChangeHealth(-explosionDamage); // Gây sát thương
                }

                var playerMovement = hit.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.Knockback(transform, knockbackForce, stunTime); // Áp dụng knockback
                }
            }
        }

        Destroy(gameObject); // Hủy TNT sau khi nổ
    }

    private void OnDrawGizmosSelected()
    {
        // Hiển thị bán kính vụ nổ trong chế độ phát triển
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
