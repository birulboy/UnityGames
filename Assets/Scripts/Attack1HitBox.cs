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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isAttacking || hasHit)
            return;

        ApplyDamage(other);
    }

    private void ApplyDamage(Collider2D other)
    {
        // BOSS
        BossHealth boss = other.GetComponentInParent<BossHealth>();

        if (boss != null)
        {
            boss.TakeDamage(damage);

            Debug.Log("Golpeé al BOSS");

            hasHit = true;
            return;
        }

        // SKELETON
        SkeletonHealth skeleton =
            other.GetComponentInParent<SkeletonHealth>();

        if (skeleton != null)
        {
            skeleton.TakeDamage(damage, isAttack2);

            Debug.Log("Golpeé un Skeleton");

            hasHit = true;
            return;
        }

        // ENEMIGOS NORMALES
        EnemyHealth enemy =
            other.GetComponentInParent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            Debug.Log("Golpeé un enemigo");

            hasHit = true;
            return;
        }
    }
}