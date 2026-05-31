using System.Collections;
using UnityEngine;

public class GirlController : MonoBehaviour
{
    [Header("Configuración")]
    public float detectionDistance = 3f;  // distancia a la que huye
    public float runSpeed = 7f;            // velocidad de huida
    public Transform player;              // arrastrás el jugador aquí

    private bool isRunning = false;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;

        // busca al jugador automáticamente si no está asignado
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (isRunning)
        {
            Run();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionDistance)
        {
            isRunning = true;
            // mira hacia la derecha al huir
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            anim.SetInteger("speedn", 1); // animación de correr
        }
    }

    void Run()
        {
            transform.position += Vector3.right * runSpeed * Time.deltaTime;

            // calcula el borde correctamente en 2D
            Vector3 screenEdge = Camera.main.ViewportToWorldPoint(
                new Vector3(1.2f, 0, Mathf.Abs(Camera.main.transform.position.z))
            );

            if (transform.position.x > screenEdge.x)
            {
                Destroy(gameObject);
            }
        }
}