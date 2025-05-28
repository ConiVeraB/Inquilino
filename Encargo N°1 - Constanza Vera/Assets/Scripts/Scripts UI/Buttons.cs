using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Buttons : MonoBehaviour
{
    public GameObject creditsPanel;
    public GameObject optionsPanel;

    public Slider musicSlider;
    public Slider SFXSlider;
    public Slider VoicesSlider; //Slider para controlar el volumen de las voces grabadas, aparte de los sonidos del juego//

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        if (creditsPanel != null && optionsPanel !=null)
        {
            creditsPanel.SetActive(false);
            optionsPanel.SetActive(false);
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
    public void ShowOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }
    }

    
     public void HideCredits()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
       
    }

    
    public void HideOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
