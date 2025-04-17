using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyTNT_Movement : MonoBehaviour
{
    public float speed;
    public float attackRange = 2;
    public float rangedAttackRange = 5; // Khoảng cách tấn công xa
    public float attackCooldown = 2;
    public float playerDetectRange = 7; // Tăng phạm vi phát hiện
    public Transform detectionPoint;
    public LayerMask playerLayer;
    private float attackCooldownTimer;
    private int facingDirection = -1;
    private EnemyState enemyState;

    private Rigidbody2D rb;
    private Transform player;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    void Update()
    {
        if (enemyState == EnemyState.Death)
            {
                rb.linearVelocity = Vector2.zero; // Dừng di chuyển
                return; // Thoát sớm, không thực hiện các logic khác
            }

        if (enemyState != EnemyState.Knockback)
        {
            CheckForPlayer();

            if (attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;
            }

            if (enemyState == EnemyState.Chasing)
            {
                Chase();
            }
            else if (enemyState == EnemyState.Attacking)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    void Chase()
    {
        if (player == null) return; // Nếu không có người chơi, không di chuyển

        // Đổi hướng đối mặt người chơi
        if (player.position.x > transform.position.x && facingDirection == -1 ||
            player.position.x < transform.position.x && facingDirection == 1)
        {
            Flip();
        }

        // Tính toán hướng di chuyển
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed; // Cập nhật vận tốc để di chuyển
    }


    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    // private void CheckForPlayer()
    // {
    //     if (enemyState == EnemyState.Death) return;

    //     Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);

    //     if (hits.Length > 0)
    //     {
    //         player = hits[0].transform;

    //         float distanceToPlayer = Vector2.Distance(transform.position, player.position);
    //         if (distanceToPlayer < attackRange && attackCooldownTimer <= 0)
    //         {
    //             attackCooldownTimer = attackCooldown;
    //             ChangeState(EnemyState.Attacking);
    //         }
    //         else if (distanceToPlayer > attackRange && distanceToPlayer < rangedAttackRange)
    //         {
    //             attackCooldownTimer = attackCooldown;
    //             ChangeState(EnemyState.Attacking); // Tấn công từ xa
    //         }
    //         else if (distanceToPlayer > rangedAttackRange)
    //         {
    //             ChangeState(EnemyState.Chasing);
    //         }
    //     }
    //     else
    //     {
    //         rb.velocity = Vector2.zero;
    //         ChangeState(EnemyState.Idle);
    //     }
    // }

    private void CheckForPlayer()
    {
        if (enemyState == EnemyState.Death) return; // Dừng kiểm tra nếu quái đã chết


        // Tìm tất cả các đối tượng trong vùng phát hiện
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);

        if (hits.Length > 0)
        {
            // Lấy vị trí người chơi đầu tiên
            player = hits[0].transform;

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Kiểm tra phạm vi và chuyển đổi trạng thái
            if (distanceToPlayer < attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                ChangeState(EnemyState.Attacking);
            }
            else if (distanceToPlayer >= attackRange && distanceToPlayer <= playerDetectRange)
            {
                ChangeState(EnemyState.Chasing); // Chuyển sang trạng thái dí theo người chơi
            }
        }
        else
        {
            // Không phát hiện người chơi, trở về trạng thái "Idle"
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }
    }


    public void ChangeState(EnemyState newState)
    {
        if (enemyState == EnemyState.Death)
        {
            return;
        }
        // Exit the current animation
        if(enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", false);
        else if(enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", false);
        else if(enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", false);
        //Update our current state
        enemyState = newState;
        //Update the new animation
        if(enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", true);
        else if(enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true);
        else if(enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", true);
        else if (enemyState == EnemyState.Death)
        {
            rb.linearVelocity = Vector2.zero; // Dừng mọi chuyển động
            anim.SetTrigger("Death");
        }
    }

    public IEnumerator RecoverFromKnockback(float recoveryTime)
{
    yield return new WaitForSeconds(recoveryTime);
    ChangeState(EnemyState.Idle);
}


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);
    }

    public enum EnemyState
    {
        Idle,
        Chasing,
        Attacking,
        Knockback,
        Death
    }
}
