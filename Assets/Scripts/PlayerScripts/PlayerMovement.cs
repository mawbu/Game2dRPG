using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }  // Singleton instance

    public int facingDirection = 1;
    public Rigidbody2D rb;
    public Animator anim;
    private bool isKnockedBack;
    public bool isShooting;
    public Player_Combat player_Combat;

    private void Awake() {
        // Đảm bảo rằng chỉ có một instance của PlayerMovement
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Update() {
        if(Input.GetButtonDown("Slash"))
        {
            player_Combat.Attack();
        }
    }

    // Fixed Update is called 50x per frame
    void FixedUpdate()
    {
        if(isShooting == true)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else if(isKnockedBack == false)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if(horizontal > 0 && transform.localScale.x < 0 ||
                horizontal < 0 && transform.localScale.x > 0)
                {
                    Flip();
                }

            anim.SetFloat("horizontal", Mathf.Abs(horizontal));
            anim.SetFloat("vertical", Mathf.Abs(vertical));

            // Sử dụng tốc độ dựa trên trạng thái hiện tại
        float movementSpeed = StatsManager.Instance.currentState == PlayerState.Sword
            ? StatsManager.Instance.swordspeed
            : StatsManager.Instance.bowspeed;

        rb.linearVelocity = new Vector2(horizontal, vertical) * movementSpeed;
        }
    }

    void Flip() {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void Knockback(Transform enemy, float force, float stunTime)
    {
        isKnockedBack = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.linearVelocity = direction * force;
        StartCoroutine(KnockbackCounter(stunTime));
    }

    IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }
}
