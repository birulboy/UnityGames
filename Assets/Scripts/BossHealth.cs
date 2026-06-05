using UnityEngine;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    public int maxHP = 200;

    private int currentHP;
    private Animator anim;
    private bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
        anim = GetComponent<Animator>();

        Debug.Log("Boss HP inicial: " + currentHP);
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;

        currentHP -= amount;

        Debug.Log("BOSS RECIBIÓ DAÑO");
        Debug.Log("HP ACTUAL: " + currentHP);

        if (currentHP > 0)
        {
            anim.SetTrigger("hitB");
        }

        if (currentHP <= 0)
        {
            isDead = true;
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        Debug.Log("ENTRÓ A DIE()");

        BossController bossController = GetComponent<BossController>();

        if (bossController != null)
        {
            bossController.enabled = false;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        // Limpiar triggers anteriores
        anim.ResetTrigger("hitB");
        anim.ResetTrigger("attackB");

        // Forzar muerte
        anim.SetTrigger("notAliveB");

        Debug.Log("TRIGGER notAliveB ACTIVADO");

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}