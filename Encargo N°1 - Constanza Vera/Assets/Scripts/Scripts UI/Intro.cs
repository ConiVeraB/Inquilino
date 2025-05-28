using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
     
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            SceneManager.LoadScene("Game");
        }
    }
}
