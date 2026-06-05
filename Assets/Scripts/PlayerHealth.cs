using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private int health = 100;
    private Animator anim;
    private bool isDead = false; // agregado

    void Start()
    {
        anim = GetComponent<Animator>();
        health = 100;
    }

    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return; // si ya está muerto ignora el daño
        
        health -= amount;
        anim.SetTrigger("hitPJ");
        Debug.Log("Jugador recibió daño, HP: " + health);

        if (health <= 0)
        {
            isDead = true; // marca como muerto
            StartCoroutine(Die());
        }   
    }

    IEnumerator Die()
    {
        anim.SetTrigger("notAlive");
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
        SceneManager.LoadScene(0);
        
    }
}