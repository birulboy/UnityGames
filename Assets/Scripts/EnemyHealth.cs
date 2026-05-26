using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    public int maxHP = 30;
    [SerializeField] private int currentHP;
     private Animator anim;

    void Start()
    {
        currentHP = maxHP;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        anim.SetTrigger("hitg");

 
        if (currentHP <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        anim.SetTrigger("notAliveg");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        
    }
}
