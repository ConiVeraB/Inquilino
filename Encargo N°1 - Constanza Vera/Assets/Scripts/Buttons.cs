using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Buttons : MonoBehaviour
{
    public GameObject creditsPanel;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("El Panel de Créditos no ha sido asignado en el Inspector para el script Buttons.");
        }
    }

    
    public void Play()
    {
        SceneManager.LoadScene("Game"); 
    }

    public void Loadscene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Volver()
    {
        SceneManager.LoadScene("Main Menú");
    }

    public void ShowCredits() 
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Por ahi no era");
        }
    }

    public void HideCredits()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Nope");
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}
