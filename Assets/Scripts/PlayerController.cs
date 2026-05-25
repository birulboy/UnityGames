using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private bool IsGrounded;
    [SerializeField] private float jumpForce = 3;
    private Animator anim;
    private bool isAttacking = false;

    [SerializeField] private int health = 100;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isAttacking)
            {
                float input = Input.GetAxisRaw("Horizontal");
                rb.linearVelocity = new Vector2(input * moveSpeed, rb.linearVelocity.y);
                anim.SetInteger("speed", (int)input);
                if (input > 0) spriteRenderer.flipX = false;
                else if (input < 0) spriteRenderer.flipX = true;
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
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                anim.SetInteger("speed", 0);
                anim.SetTrigger("attack1");
                StartCoroutine(ResetAttack1());
            }
        //ataque2
        // if (Input.GetKeyDown(KeyCode.K) && !isAttacking)
        //     {
        //         isAttacking = true;
        //         rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        //         anim.SetInteger("speed", 0);
        //         anim.SetTrigger("attack2");
        //         StartCoroutine(ResetAttack2());
        //     }
        if ( health <= 0)
            {
                anim.SetTrigger("notAlive");
                // Aquí podrías agregar lógica para reiniciar el nivel o mostrar una pantalla de Game Over
            }
    }

    IEnumerator ResetAttack1()
        {
            // Esperás que termine la animación de ataque
            yield return new WaitForSeconds(0.214f);
            isAttacking = false;
        }
      IEnumerator ResetAttack2()
        {
            // Esperás que termine la animación de ataque
            yield return new WaitForSeconds(0.700f);
            isAttacking = false;
        }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IsGrounded = true;
        anim.SetInteger("height", 0);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        IsGrounded = false;
    }
}
