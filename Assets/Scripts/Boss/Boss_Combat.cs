using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Combat : MonoBehaviour
{
    public int damage = 1;
    public Transform attackPoint;
    public float weaponRange;
    public float knockbackForce;
    public float stunTime;
    public LayerMask playerLayer;

    [SerializeField] private AudioClip bossSound; // Âm thanh khi tấn công

    public void Attack()
    {
        float scaleFactor = transform.localScale.x; // Assuming uniform scaling
        float adjustedWeaponRange = weaponRange * scaleFactor;
        
        if (bossSound != null)
        {
            SoundManager.instance.PlaySound(bossSound);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);
        

        foreach (Collider2D hit in hits)
        {
            
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                
                playerHealth.ChangeHealth(-damage);
                PlayerMovement playerMovement = hit.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.Knockback(transform, knockbackForce, stunTime);
                }
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
    }
}
