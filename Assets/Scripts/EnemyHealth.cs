using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    public int maxHP = 30;
    [SerializeField] private int currentHP;
    private Animator anim;
    private EnemyController enemyController;
    private bool isDead = false; // nuevo - evita daño después de morir

    void Start()
    {
        currentHP = maxHP;
        anim = GetComponent<Animator>();
        enemyController = GetComponent<EnemyController>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return; // nuevo

        enemyController.CancelAttack(); // cancela ataque en curso
        
        currentHP -= amount;
        anim.SetTrigger("hitg");
        StartCoroutine(StunEnemy());

        if (currentHP <= 0)
        {
            isDead = true; // nuevo
            StartCoroutine(Die());
        }
    }

    IEnumerator StunEnemy()
    {
        if (enemyController != null)
        {
            enemyController.isStunned = true;
            yield return new WaitForSeconds(1f);
            if (!isDead) // solo libera el stun si no está muerto
                enemyController.isStunned = false;
        }
    }

    IEnumerator Die()
    {
        anim.SetTrigger("notAliveg");
        yield return new WaitForSeconds(1f);

        GameManager.Instance.score += 5;
        Destroy(gameObject);
    }
}