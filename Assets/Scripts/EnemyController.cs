using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movimiento")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public float patrolDistance = 3f; // distancia que patrulla desde su posición inicial

    [Header("Ataque")]
    public float attackCooldown = 1.5f;
    public float stunDuration = 0.5f;
    public int damage = 10;

    [Header("Estado")]
    public bool playerInRange = false;
    public Transform playerTransform;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isOnCooldown = false;
    private bool isStunned = false;

    private Vector3 startPosition;
    private int patrolDirection = 1; // 1 derecha, -1 izquierda

    private Vector3 originalScale;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
        originalScale = transform.localScale; // agregado
    }

    void Update()
    {
        if (isStunned) return;

        if (playerInRange && playerTransform != null)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        anim.SetInteger("speedg", 1);

        // voltea cuando llega al límite de patrulla
        if (transform.position.x >= startPosition.x + patrolDistance)
            patrolDirection = -1;
        else if (transform.position.x <= startPosition.x - patrolDistance)
            patrolDirection = 1;

        rb.linearVelocity = new Vector2(patrolDirection * patrolSpeed, rb.linearVelocity.y);
        transform.localScale = new Vector3(patrolDirection * Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

    }

    void ChasePlayer()
    {
        anim.SetInteger("speedg", 1);

        float direction = playerTransform.position.x > transform.position.x ? 1 : -1;
        rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);
        transform.localScale = new Vector3(direction * Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    public void TryAttackPlayer(Collider2D playerCollider)
    {
        if (isOnCooldown) return;
        StartCoroutine(AttackPlayer(playerCollider));
    }

    IEnumerator AttackPlayer(Collider2D playerCollider)
    {
        isOnCooldown = true;
        isStunned = true; // se queda quieto mientras ataca
        rb.linearVelocity = Vector2.zero;

        anim.SetTrigger("attack1g");
        yield return new WaitForSeconds(0.5f);

        PlayerController player = playerCollider.GetComponent<PlayerController>();
        if (player != null)
        {
            yield return StartCoroutine(StunPlayer(player));
        }

        isStunned = false;
        yield return new WaitForSeconds(attackCooldown - 0.5f);
        isOnCooldown = false;
    }

    IEnumerator StunPlayer(PlayerController player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.TakeDamage(damage);

        player.isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        player.isStunned = false;
    }
}