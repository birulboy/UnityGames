using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movimiento")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public float patrolDistance = 3f;

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
    public bool isStunned = false;

    private Vector3 startPosition;
    private int patrolDirection = 1;
    private Vector3 originalScale;
    private Coroutine attackCoroutine; // nuevo

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isStunned) return;

        if (playerInRange && playerTransform != null)
            ChasePlayer();
        else
            Patrol();
    }

    void Patrol()
    {
        anim.SetInteger("speedg", 1);

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
        attackCoroutine = StartCoroutine(AttackPlayer(playerCollider));
    }

    public void CancelAttack() // nuevo - cancela el ataque en curso
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        isStunned = false;
        isOnCooldown = false;
        rb.linearVelocity = Vector2.zero;
    }

    IEnumerator AttackPlayer(Collider2D playerCollider)
    {
        isOnCooldown = true;
        isStunned = true;
        rb.linearVelocity = Vector2.zero;

        anim.SetTrigger("attack1g");
        yield return new WaitForSeconds(0.5f);

        // verifica que el jugador sigue vivo antes de hacer daño
        if (playerCollider != null)
        {
            PlayerController player = playerCollider.GetComponent<PlayerController>();
            if (player != null)
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

    void OnDestroy() // nuevo - fix stun infinito al morir
    {
        PlayerController playerController = GameObject.FindWithTag("Player")?.GetComponent<PlayerController>();
        if (playerController != null)
            playerController.isStunned = false;
    }
}