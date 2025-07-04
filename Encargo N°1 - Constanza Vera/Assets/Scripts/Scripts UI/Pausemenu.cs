using UnityEngine;
using UnityEngine.SceneManagement;

public class Pausemenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private bool isPaused = false;
    public static bool GameIsPaused { get; set; }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void Home()
    { // --- AÑADE ESTAS LÍNEAS PARA RESETEAR EL ESTADO ---
        isPaused = false;
        GameIsPaused = false;
        // --- FIN DE LÍNEAS A AÑADIR ---
        SceneManager.LoadScene("Main Menú");
        Time.timeScale = 1;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        GameIsPaused = false;
        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }


}
