using UnityEngine;
using UnityEngine.SceneManagement;
public class Buttons : MonoBehaviour
{
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

    public void Quit()
    {
        Application.Quit();
    }
}
