using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndZone : MonoBehaviour
{
    private bool finished = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (finished)
            return;

        Transform player = other.transform.root;

        if (player.CompareTag("Player"))
        {
            finished = true;

            StartCoroutine(FinishGame(player.gameObject));
        }
    }

    IEnumerator FinishGame(GameObject player)
    {
        player.SetActive(false);

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("5");
    }
}