using UnityEngine;

public class Attack1HitBox : MonoBehaviour
{
    public int damage = 10;
<<<<<<< HEAD

=======
    public bool isAttack2 = false;
>>>>>>> abfe308ed42e873855e52b5d885d8fe605b3f389
    private bool isAttacking = false;
    private bool hasHit = false;

    public void EnableHitbox()
    {
        isAttacking = true;
<<<<<<< HEAD
=======
        hasHit = false;
>>>>>>> abfe308ed42e873855e52b5d885d8fe605b3f389
    }

    public void DisableHitbox()
    {
        isAttacking = false;
        hasHit = false;
    }

<<<<<<< HEAD
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isAttacking)
            return;

        // Enemigos normales
        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            Debug.Log("Golpeé un enemigo");

            isAttacking = false;
            return;
        }

        // Boss
        BossHealth boss = other.GetComponentInParent<BossHealth>();

        if (boss != null)
        {
            boss.TakeDamage(damage);

            Debug.Log("Golpeé al BOSS");

            isAttacking = false;
            return;
=======
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
>>>>>>> abfe308ed42e873855e52b5d885d8fe605b3f389
        }
    }
}