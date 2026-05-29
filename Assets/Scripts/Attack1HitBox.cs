using UnityEngine;

public class Attack1HitBox : MonoBehaviour
{
    public int damage = 10;
    public bool isAttack2 = false;
    private bool isAttacking = false;
    private bool hasHit = false;

    public void EnableHitbox()
    {
        isAttacking = true;
        hasHit = false;
    }

    public void DisableHitbox()
    {
        isAttacking = false;
        hasHit = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (isAttacking && !hasHit && other.CompareTag("Enemy"))
            ApplyDamage(other);
    }

    private void ApplyDamage(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            hasHit = true;
            return;
        }

        SkeletonHealth skeleton = other.GetComponent<SkeletonHealth>();
        if (skeleton != null)
        {
            skeleton.TakeDamage(damage, isAttack2);
            hasHit = true;
        }
    }
}