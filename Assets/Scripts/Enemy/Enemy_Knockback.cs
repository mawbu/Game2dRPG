using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Knockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private Enemy_Movement enemy_Movement;
    private EnemyTNT_Movement enemyTNT_Movement;



    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        enemy_Movement = GetComponent<Enemy_Movement>();
        enemyTNT_Movement = GetComponent<EnemyTNT_Movement>();
    }
    public void Knockback(Transform forceTransform, float knockbackForce, float knockbackTime, float stunTime)
    {
        enemy_Movement.ChangeState(Enemy_Movement.EnemyState.Knockback);
        //enemyTNT_Movement.ChangeState(EnemyTNT_Movement.EnemyState.Knockback);
        StartCoroutine(StunTimer(knockbackTime, stunTime));
        Vector2 direction = (transform.position - forceTransform.position).normalized;
        rb.linearVelocity = direction * knockbackForce;
        Debug.Log("knockback applied.");
    }

    IEnumerator StunTimer(float knockbackTime, float stunTime)
    {
        yield return new  WaitForSeconds(knockbackTime);
        rb.linearVelocity = Vector2.zero;
        yield return new  WaitForSeconds(stunTime);
        enemy_Movement.ChangeState(Enemy_Movement.EnemyState.Idle);
        //enemyTNT_Movement.ChangeState(EnemyTNT_Movement.EnemyState.Idle);
    }
}
