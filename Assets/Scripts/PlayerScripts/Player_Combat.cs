using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{

    public Transform attackPoint;
    public LayerMask enemyLayer;
    public StatsUI statsUI;
    public Animator anim;
    public float cooldown = 2;
    private float timer;
    [SerializeField] private AudioClip slashSound;
    private bool isEnabled = true; // Thêm cờ kiểm soát trạng thái

    private void Update() {

        if (!isEnabled) return; // Ngăn xử lý nếu không ở trạng thái kiếm

        if(timer > 0 )
        {
            timer -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        
        if (!isEnabled) return; // Ngăn tấn công nếu không phải trạng thái kiếm

        if(timer <= 0)
        {
            anim.SetBool("isAttacking", true);
            timer = cooldown;
            SoundManager.instance.PlaySound(slashSound);
        }
    }

        private void OnEnable()
    {
        StatsManager.Instance.UpdateStats(PlayerState.Sword); // Cập nhật trạng thái kiếm
    }


    public void CancelAttack()
    {
        if (!isEnabled) return; // Chỉ hủy nếu đang ở trạng thái kiếm

        anim.SetBool("isAttacking", false);
        timer = 0; // Reset thời gian cooldown
    }

    public void DisableCombat()
    {
        isEnabled = false; // Vô hiệu hóa trạng thái kiếm
        CancelAttack(); // Hủy bất kỳ đòn đánh nào
    }

    public void EnableCombat()
    {
        isEnabled = true; // Kích hoạt lại trạng thái kiếm
    }

    public void DealDamage()
{
    Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, StatsManager.Instance.weaponRange, enemyLayer);

    foreach (var enemy in enemies)
    {
        if (enemy == null) continue;

        // Kiểm tra và xử lý quái thường
        var destructible = enemy.GetComponent<Destructible>();
        if (destructible != null)
        {
            destructible.DestroyWithEffect();
            break;
        }

        // Kiểm tra quái có Enemy_Health
        var enemyHealth = enemy.GetComponent<Enemy_Health>();
        
        if (enemyHealth != null )
        {
            enemyHealth.ChangeHealth(-StatsManager.Instance.sworddamage);

            // Xử lý Knockback cho quái thường
            var enemyKnockback = enemy.GetComponent<Enemy_Knockback>();
            if (enemyKnockback != null)
            {
                enemyKnockback.Knockback(transform, StatsManager.Instance.knockbackForce, StatsManager.Instance.knockbackTime, StatsManager.Instance.stunTime);
                continue;
            }

            // Xử lý Knockback cho quái đánh xa
            var enemyTNTKnockback = enemy.GetComponent<EnemyTNT_Knockback>();
            if (enemyTNTKnockback != null)
            {
                enemyTNTKnockback.Knockback(transform, StatsManager.Instance.knockbackForce, StatsManager.Instance.knockbackTime, StatsManager.Instance.stunTime);
            }
        }
        var bossHealth = enemy.GetComponent<Boss_Health>();
        if(bossHealth != null)
        {

            bossHealth.ChangeHealth(-StatsManager.Instance.sworddamage);
        }
    }
}


    public void FinishAttacking()
    {
        anim.SetBool("isAttacking", false);
    }

    // private void OnDrawGizmosSelected() {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(attackPoint.position, StatsManager.Instance.weaponRange);
    // }
}
