using UnityEngine;

public class SkeletonAttack : MonoBehaviour
{
    private SkeletonController skeletonController;

    void Start()
    {
        skeletonController = GetComponentInParent<SkeletonController>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            skeletonController.TryAttackPlayer(other);
        }
    }
}