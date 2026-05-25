using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ClickToStart : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame || 
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene(1);
        }
    }
}