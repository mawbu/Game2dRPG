using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public int exReward = 3;
    public delegate void MonsterDefeated(int exp);
    public static event MonsterDefeated OnMonsterDefeated;
    public int currentHealth;
    public int maxHealth;
    public Animator anim;
    private Enemy_Movement enemy_Movement;
    private EnemyTNT_Movement enemyTNT_Movement;
    private bool isDead = false;

    private void Start() {
        currentHealth = maxHealth;
        enemy_Movement = GetComponent<Enemy_Movement>();
        enemyTNT_Movement = GetComponent<EnemyTNT_Movement>();
        
    }

    public void ChangeHealth(int amount)
    {
        if (isDead) return; // Nếu boss đã chết, không thực hiện
        currentHealth += amount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
                else if (currentHealth <= 0)
        {
            isDead = true;

            if (anim != null)
            {
                anim.SetTrigger("Death");
            }

            if (enemy_Movement != null)
            {
                enemy_Movement.ChangeState(Enemy_Movement.EnemyState.Death);
            }

            if (enemyTNT_Movement != null)
            {
                enemyTNT_Movement.ChangeState(EnemyTNT_Movement.EnemyState.Death);
            }

            var rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero; // Dừng mọi chuyển động
            }

            OnMonsterDefeated?.Invoke(exReward);
            StartCoroutine(HandleDestroyAfterDeath());
        }
    }

    private IEnumerator HandleDestroyAfterDeath()
    {
        // Chờ thời gian tương đương với độ dài animation Death (hoặc tùy chỉnh)
        yield return new WaitForSeconds(5.0f); // Điều chỉnh thời gian theo animation
        Destroy(gameObject);
    }
}
