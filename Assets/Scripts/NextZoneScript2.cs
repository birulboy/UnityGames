using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class NextZoneScript2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            
             SceneManager.LoadScene(4);
        }
    }
}
