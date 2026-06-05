using System.Collections;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    [Header("Movimiento")]
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 2.5f;
    public float patrolDistance = 3f;

    [Header("Ataque")]
    public float attackCooldown = 4f;
    public float stunDuration = 0.5f;
    public int damage = 15;

    [Header("Bloqueo")]
    public bool isBlocking = false;
    public float blockDuration = 3f;

    [Header("Estado")]
    public bool playerInRange = false;
    public Transform playerTransform;
    public bool isStunned = false;

    private Animator anim;
    private Rigidbody2D rb;
    private bool isAttackOnCooldown = false;
    private Vector3 startPosition;
    private int patrolDirection = 1;
    private Vector3 originalScale;
    private Coroutine attackCoroutine;
    private Coroutine blockCoroutine;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isStunned || isBlocking) return;

        if (playerInRange && playerTransform != null)
            ChasePlayer();
        else
            Patrol();
    }

    void Patrol()
    {
        anim.SetInteger("speedE", 1);

        if (transform.position.x >= startPosition.x + patrolDistance)
            patrolDirection = -1;
        else if (transform.position.x <= startPosition.x - patrolDistance)
            patrolDirection = 1;

        rb.linearVelocity = new Vector2(patrolDirection * patrolSpeed, rb.linearVelocity.y);
        transform.localScale = new Vector3(patrolDirection * Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    void ChasePlayer()
    {
        anim.SetInteger("speedE", 1);

        float direction = playerTransform.position.x > transform.position.x ? 1 : -1;
        rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);
        transform.localScale = new Vector3(direction * Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    public void TryAttackPlayer(Collider2D playerCollider)
    {
        if (isBlocking || isAttackOnCooldown || isStunned) return;
        attackCoroutine = StartCoroutine(AttackPlayer(playerCollider));
    }

    // llamado desde SkeletonHealth cuando recibe J
    public void ActivateBlock()
    {

        // cancela el ataque en curso si estaba atacando
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
            isStunned = false;
        }

        if (blockCoroutine != null)
            StopCoroutine(blockCoroutine);

        blockCoroutine = StartCoroutine(BlockRoutine());
    }

    IEnumerator BlockRoutine()
    {
        isBlocking = true;
        rb.linearVelocity = Vector2.zero;
        anim.SetInteger("speedE", 0);
        anim.SetTrigger("blockE");

        yield return new WaitForSeconds(blockDuration); // 3s quieto

        isBlocking = false;
    }

    public void CancelAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        isStunned = false;
        isAttackOnCooldown = false;
        rb.linearVelocity = Vector2.zero;
    }

    IEnumerator AttackPlayer(Collider2D playerCollider)
    {
        isAttackOnCooldown = true;
        isStunned = true;
        rb.linearVelocity = Vector2.zero;

        anim.SetTrigger("attackE");
        yield return new WaitForSeconds(0.525f);

        if (playerCollider != null)
        {
            PlayerController player = playerCollider.GetComponent<PlayerController>();
            if (player != null)
                yield return StartCoroutine(StunPlayer(player));
        }

        isStunned = false;
        yield return new WaitForSeconds(attackCooldown); // 4s entre ataques
        isAttackOnCooldown = false;
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

    void OnDestroy()
    {
        PlayerController playerController = GameObject.FindWithTag("Player")?.GetComponent<PlayerController>();
        if (playerController != null)
            playerController.isStunned = false;
    }
}