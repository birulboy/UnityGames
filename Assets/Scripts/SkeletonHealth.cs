using UnityEngine;
using System.Collections;

public class SkeletonHealth : MonoBehaviour
{
    public int maxHP = 50;
    [SerializeField] private int currentHP;
    private Animator anim;
    private SkeletonController skeletonController;
    private bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
        anim = GetComponent<Animator>();
        skeletonController = GetComponent<SkeletonController>();
    }

    public void TakeDamage(int amount, bool isAttack2 = false)
    {
        if (isDead) return;

        if (!isAttack2)
        {
            // J siempre activa el bloqueo, incluso después de uno anterior
            skeletonController.ActivateBlock();
            return;
        }

        // K hace daño
        skeletonController.CancelAttack();
        currentHP -= amount;
        StartCoroutine(HitReaction());

        if (currentHP <= 0)
        {
            isDead = true;
            StartCoroutine(Die());
        }
    }

    IEnumerator HitReaction()
    {
        skeletonController.isStunned = true;
        skeletonController.isBlocking = false; // cancela bloqueo si estaba activo
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("hitE");
        Debug.Log("hitE activado");
        yield return new WaitForSeconds(1f);
        if (!isDead)
            skeletonController.isStunned = false;
    }

    IEnumerator Die()
    {
        skeletonController.isStunned = true;
        anim.SetTrigger("notAliveE");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}