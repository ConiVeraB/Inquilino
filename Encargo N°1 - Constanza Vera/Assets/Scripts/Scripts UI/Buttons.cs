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
      
    }

    public void Play()
    {
        SceneManager.LoadScene("Transicion"); 
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
        
    }

     public void HideCredits()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
       
    }


    public void Quit()
    {
        Application.Quit();
    }
}
