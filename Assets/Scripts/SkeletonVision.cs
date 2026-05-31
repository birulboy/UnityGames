using UnityEngine;

public class SkeletonVision : MonoBehaviour
{
    private SkeletonController skeletonController;

    void Start()
    {
        skeletonController = GetComponentInParent<SkeletonController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            skeletonController.playerInRange = true;
            skeletonController.playerTransform = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            skeletonController.playerInRange = false;
            skeletonController.playerTransform = null;
        }
    }
}