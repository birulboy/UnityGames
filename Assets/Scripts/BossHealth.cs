using UnityEngine;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    public int maxHP = 200;

    private int currentHP;
    private Animator anim;

    void Start()
    {
        currentHP = maxHP;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("BOSS RECIBIÓ DAÑO");

        currentHP -= amount;

        anim.SetTrigger("hitB");

        if (currentHP <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        anim.SetTrigger("notAliveB");

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}