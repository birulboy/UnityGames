using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Proyectil")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Movimiento")]
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 3f;
    public float patrolDistance = 4f;

    [Header("Ataque")]
    public float attackCooldown = 2f;
    public float stunDuration = 1f;
    public int damage = 20;

    [Header("Estado")]
    public bool playerInRange = false;
    public Transform playerTransform;

    private Animator anim;
    private Rigidbody2D rb;

    private bool isOnCooldown = false;
    private bool isStunned = false;

    private Vector3 startPosition;
    private int patrolDirection = 1;
    private Vector3 originalScale;

    void Start()
    {
        Debug.Log("BossController cargado");

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        startPosition = transform.position;
        originalScale = transform.localScale;
    }
    void Update()
    {
        if (isStunned)
            return;

        if (playerInRange && playerTransform != null)
            ChasePlayer();
        else
            Patrol();
    }

    void Patrol()
    {
        anim.SetFloat("speedB", 1);

        if (transform.position.x >= startPosition.x + patrolDistance)
            patrolDirection = -1;
        else if (transform.position.x <= startPosition.x - patrolDistance)
            patrolDirection = 1;

        rb.linearVelocity = new Vector2(
            patrolDirection * patrolSpeed,
            rb.linearVelocity.y
        );

        transform.localScale = new Vector3(
            patrolDirection * Mathf.Abs(originalScale.x),
            originalScale.y,
            originalScale.z
        );
    }

    void ChasePlayer()
    {
        anim.SetFloat("speedB", 1);

        float direction =
            playerTransform.position.x > transform.position.x
            ? 1
            : -1;

        rb.linearVelocity = new Vector2(
            direction * chaseSpeed,
            rb.linearVelocity.y
        );

        transform.localScale = new Vector3(
            direction * Mathf.Abs(originalScale.x),
            originalScale.y,
            originalScale.z
        );
    }

    public void TryAttackPlayer(Collider2D playerCollider)
    {
        if (isOnCooldown)
            return;

        if (playerTransform == null)
            return;

        StartCoroutine(AttackPlayer());
    }

    IEnumerator AttackPlayer()
    {
        isOnCooldown = true;
        isStunned = true;

        rb.linearVelocity = Vector2.zero;

        anim.SetFloat("speedB", 0);

        // Inicia la animación completa
        anim.Play("attackB", 0, 0f);

        // Espera para que se vea levantar los brazos
        yield return new WaitForSeconds(1.2f);

        ShootProjectile();

        // Espera para que termine la animación
        yield return new WaitForSeconds(1f);

        isStunned = false;

        yield return new WaitForSeconds(attackCooldown);

        isOnCooldown = false;
    }

    public void ShootProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile Prefab no asignado");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("FirePoint no asignado");
            return;
        }

        if (playerTransform == null)
        {
            return;
        }

        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        Vector2 direction =
            (playerTransform.position - firePoint.position).normalized;

        BossProjectile projectileScript =
            projectile.GetComponent<BossProjectile>();

        if (projectileScript != null)
        {
            projectileScript.SetDirection(direction);
        }
    }
}