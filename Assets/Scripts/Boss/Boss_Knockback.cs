using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Knockback : MonoBehaviour
{
private Rigidbody2D rb;
    // private Enemy_Movement enemy_Movement;
    private Boss_Movement boss_Movement;



    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        boss_Movement = GetComponent<Boss_Movement>();
    }
    public void Knockback(Transform playerTransform, float knockbackForce, float knockbackTime, float stunTime)
    {
        boss_Movement.ChangeState(Boss_Movement.EnemyState.Knockback);
        StartCoroutine(StunTimer(knockbackTime, stunTime));
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        rb.linearVelocity = direction * knockbackForce;
        // Debug.Log("knockback applied.");
    }

    IEnumerator StunTimer(float knockbackTime, float stunTime)
    {
        yield return new  WaitForSeconds(knockbackTime);
        rb.linearVelocity = Vector2.zero;
        yield return new  WaitForSeconds(stunTime);
        boss_Movement.ChangeState(Boss_Movement.EnemyState.Idle);
    }
}
