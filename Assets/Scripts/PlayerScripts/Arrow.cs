using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 direction = Vector2.right;
    public float lifeSpawn = 2;
    public float speed;
    public LayerMask enemyLayer;
    public LayerMask obstacleLayer;
    public LayerMask invisibleWallLayer;
    public SpriteRenderer sr;
    public Sprite buriedSprite;
    public float knockbackForce;
    public float knockbackTime;
    public float stunTime;
    // Start is called before the first frame update
    void Start()
    {
        rb.linearVelocity = direction * speed;
        RotateArrow();
        // Bỏ qua va chạm giữa lớp Arrow và lớp InvisibleWallLayer
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("InvisibleWallLayer"));
        Destroy(gameObject, lifeSpawn);
    }

    private void RotateArrow()
    {
        float angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnCollisionEnter2D(Collision2D collision) 
{
    // Kiểm tra nếu đối tượng thuộc "Enemy" layer
    if ((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
    {
        // Kiểm tra và gây sát thương cho Enemy_Health
        var enemyHealth = collision.gameObject.GetComponent<Enemy_Health>();
        if (enemyHealth != null)
        {
            enemyHealth.ChangeHealth(-StatsManager.Instance.bowdamage);
        }

        // Kiểm tra và gây sát thương cho Boss_Health
        var bossHealth = collision.gameObject.GetComponent<Boss_Health>();
        if (bossHealth != null)
        {
            Debug.Log("Boss detected, applying damage.");
            bossHealth.ChangeHealth(-StatsManager.Instance.bowdamage); // Gây sát thương cho boss
        }
        AttachToTarger(collision.gameObject.transform);
    }
    else if ((obstacleLayer.value & (1 << collision.gameObject.layer)) > 0)
    {
        AttachToTarger(collision.gameObject.transform);
    }
    var destructible = collision.gameObject.GetComponent<Destructible>();
    if (destructible != null)
    {
        destructible.DestroyWithEffect(); // Gọi phương thức phá hủy với hiệu ứng
    }
}


    private void AttachToTarger(Transform target)
    {
        sr.sprite = buriedSprite;
        rb.linearVelocity = Vector2.zero; // Ngừng chuyển động của mũi tên
        rb.isKinematic = true; // Vô hiệu hóa tác động vật lý của mũi tên
        rb.simulated = false; // Ngăn mũi tên tương tác vật lý với đối tượng

        transform.SetParent(target);
    }

}
