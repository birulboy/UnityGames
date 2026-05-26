using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyController enemyController;

    void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("jugador en el hitbox");
        if (other.CompareTag("Player"))
        {
            if (enemyController != null)
                enemyController.TryAttackPlayer(other);
                Debug.Log("Intentando atacar al jugador");
            
        }
    }
}