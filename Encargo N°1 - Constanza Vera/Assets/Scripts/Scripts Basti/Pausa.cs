using UnityEngine;
using UnityEngine.UI;

public class Pausa : MonoBehaviour
{
    public GameObject pausePanel;
    public MonoBehaviour cameraScript; // Asigna el script de cámara si quieres pausarla también
    private bool isGamePaused = false;

    private void Start()
    {
        Time.timeScale = 1;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isGamePaused = !isGamePaused;
        ApplyPauseState();
    }

    public void ResumeGame() // Este método se usará en el botón "Reanudar"
    {
        Debug.Log("Botón Reanudar presionado");
        isGamePaused = false;
        ApplyPauseState();
    }

    private void ApplyPauseState()
    {
        if (isGamePaused)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            if (cameraScript != null)
                cameraScript.enabled = false;
        }
        else
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            if (cameraScript != null)
                cameraScript.enabled = true;
        }
    }

}

