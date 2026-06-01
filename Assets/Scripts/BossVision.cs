using UnityEngine;

public class BossVision : MonoBehaviour
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
            Debug.Log("BossVision iniciado");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entró: " + other.name + " Tag: " + other.tag);

        Transform player = other.transform.root;

        if (player.CompareTag("Player"))
        {
            Debug.Log("PLAYER DETECTADO");

            bossController.playerInRange = true;
            bossController.playerTransform = player;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Transform player = other.transform.root;

        if (player.CompareTag("Player"))
        {
            Debug.Log("PLAYER SALIÓ");

            bossController.playerInRange = false;
            bossController.playerTransform = null;
        }
    }
}