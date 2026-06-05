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
        }
    }
}