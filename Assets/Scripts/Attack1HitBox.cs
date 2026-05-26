using UnityEngine;


public class Attack1HitBox : MonoBehaviour
{
    public int damage = 10;
    private bool isAttacking = false;

    public void EnableHitbox()
    {
        isAttacking = true;
        
    }

    public void DisableHitbox()
    {
        isAttacking = false;
    }
    void OnTriggerStay2D(Collider2D other )
    {
        if (isAttacking && other.CompareTag("Enemy"))
         {
            ApplyDamage(other);
           
         }
    }
    private void ApplyDamage(Collider2D other)
    {
    
    EnemyHealth enemy = other.GetComponent<EnemyHealth>();
    if (enemy != null)
        {
            enemy.TakeDamage(damage);
           
            isAttacking = false; // 
        }
    
    }
}

