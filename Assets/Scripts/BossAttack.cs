using UnityEngine;

public class BossAttack : MonoBehaviour
{
    private BossController bossController;

    void Start()
    {
        bossController = FindFirstObjectByType<BossController>();

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
        Transform player = other.transform.root;

        if (player.CompareTag("Player"))
        {
            bossController.TryAttackPlayer(other);
        }
    }
}