using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ClickToStart : MonoBehaviour
{
    public TextMeshProUGUI pressAnyKeyText; 
    public AudioSource musicSource;         
    public float blinkSpeed = 2f;         // velocidad del parpadeo

    void Start()
    {
        StartCoroutine(BlinkText());

        if (musicSource != null)
            musicSource.Play();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(1);
        }
    }

    IEnumerator BlinkText()
    {
        while (true)
        {
            pressAnyKeyText.alpha = 1f;
            yield return new WaitForSeconds(blinkSpeed);
            pressAnyKeyText.alpha = 0f;
            yield return new WaitForSeconds(blinkSpeed/4);
        }
    }
}