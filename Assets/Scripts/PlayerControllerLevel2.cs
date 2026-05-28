using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class PlayerController2 : MonoBehaviour
{
    private float moveSpeed = 5;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer hitboxRender;
    [SerializeField] private bool IsGrounded;
    [SerializeField] private float jumpForce = 4;
    private Animator anim;
    private bool isAttacking = false;
    public bool isStunned = false; // agregado

    [SerializeField] private Attack1HitBox attack1Hitbox;

    private Vector3 originalScale;

    private Collider2D myCollider;

    private void Awake()
    {
        myCollider = GetComponent<Collider2D>(); // Collider del player, no hijos
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        attack1Hitbox = GetComponentInChildren<Attack1HitBox>();
        hitboxRender = attack1Hitbox.GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        

        if (!isAttacking && !isStunned)
            {
                float input = Input.GetAxisRaw("Horizontal");
                rb.linearVelocity = new Vector2(input * moveSpeed, rb.linearVelocity.y);
                anim.SetInteger("speed", (int)input);
                if (input > 0) transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
                else if (input < 0) transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
        //salto
        if (Input.GetButtonDown("Jump") && IsGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                anim.SetInteger("height", 1);
            }
        //ataque 1
        if (Input.GetKeyDown(KeyCode.J) && !isAttacking)
            {
                isAttacking = true;
                attack1Hitbox.EnableHitbox();
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                anim.SetInteger("speed", 0);
                anim.SetTrigger("attack1");
                StartCoroutine(ResetAttack1());
            }
        //ataque2
        if (Input.GetKeyDown(KeyCode.K) && !isAttacking)
            {
                isAttacking = true;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                anim.SetInteger("speed", 0);
                anim.SetTrigger("attack2");
                StartCoroutine(ResetAttack2());
            }
    }

    IEnumerator ResetAttack1()
        {
            yield return new WaitForSeconds(0.214f);
            attack1Hitbox.DisableHitbox();
            isAttacking = false;
        }
    IEnumerator ResetAttack2()
        {
            yield return new WaitForSeconds(0.700f);
            isAttacking = false;
        }

    private void OnCollisionEnter2D(Collision2D other)
    {   
        // Debug.log("Resivsando ENTRADA a hitbox");
        if (other.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
            anim.SetInteger("height", 0);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // Debug.log("Resivsando SALIDA a hitbox");
        if (other.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("SlowBush")) return;

        // ¿Cuál de mis colliders tocó esto?
        // Verificamos que el arbusto esté tocando MI collider, no el hijo
        if (other.IsTouching(myCollider))
        {
            Debug.Log("Entrando arbusto - collider del player");
            moveSpeed = 2;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("SlowBush")) return;

        if (!other.IsTouching(myCollider))
        {
            Debug.Log("Saliendo arbusto - collider del player");
            moveSpeed = 5;
        }
    }
}