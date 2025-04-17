using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTNT_Knockback : MonoBehaviour
{
    private Rigidbody2D rb;
    // private Enemy_Movement enemy_Movement;
    private EnemyTNT_Movement enemyTNT_Movement;



    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        enemyTNT_Movement = GetComponent<EnemyTNT_Movement>();
    }
    public void Knockback(Transform forceTransform, float knockbackForce, float knockbackTime, float stunTime)
{
    if (enemyTNT_Movement != null)
    {
        enemyTNT_Movement.ChangeState(EnemyTNT_Movement.EnemyState.Knockback);
    }

    enemyTNT_Movement.ChangeState(EnemyTNT_Movement.EnemyState.Knockback);

    StartCoroutine(StunTimer(knockbackTime, stunTime));
    Vector2 direction = (transform.position - forceTransform.position).normalized;
    rb.linearVelocity = direction * knockbackForce;
    Debug.Log("Knockback applied.");
}


    IEnumerator StunTimer(float knockbackTime, float stunTime)
{
    yield return new WaitForSeconds(knockbackTime);
    rb.linearVelocity = Vector2.zero;
    if (enemyTNT_Movement != null)
    {
        StartCoroutine(enemyTNT_Movement.RecoverFromKnockback(stunTime));
    }
    enemyTNT_Movement.ChangeState(EnemyTNT_Movement.EnemyState.Idle);
}

}
