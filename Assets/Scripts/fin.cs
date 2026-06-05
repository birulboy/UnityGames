using UnityEngine;

public class CreditsScroller : MonoBehaviour
{
    public float speed = 50f;

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}