using UnityEngine;

public class BossAttack : MonoBehaviour
{
    private BossController bossController;

    void Start()
    {
        // Busca el BossController en toda la escena
        bossController = FindAnyObjectByType<BossController>();

        if (bossController == null)
        {
            Debug.LogError("No encontré BossController");
        }
        else
        {
            Debug.Log("BossAttack conectado");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Si el boss ya murió, evita errores
        if (bossController == null)
            return;

        // Obtiene el objeto principal que entró al trigger
        Transform player = other.transform.root;

        // Si es el jugador, intenta atacar
        if (player.CompareTag("Player"))
        {
            bossController.TryAttackPlayer(other);
        }
    }

    private void OnDisable()
    {
        bossController = null;
    }

    private void OnDestroy()
    {
        bossController = null;
    }
}