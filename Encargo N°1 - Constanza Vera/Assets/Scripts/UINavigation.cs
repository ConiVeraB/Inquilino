using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement; // ¡MUY IMPORTANTE! Necesario para cambiar de escena.

public class UINavigation : MonoBehaviour
{
    // Esta función pública la llamaremos desde el botón.
    // Recibe el nombre de la escena que queremos cargar.

    void Start()
    {
        // Aseguramos el estado correcto para cualquier escena de menú o final.
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None; // Liberar el cursor.
        Cursor.visible = true;                  // Hacerlo visible.
    }
    // --- FIN DEL MÉTODO A AÑADIR --
    public void LoadSceneByName(string MainMenú)
    {
        // Imprime un mensaje en la consola para saber que funciona.
        Debug.Log("Cargando escena: " + MainMenú);

        // La línea mágica que carga la escena.
        SceneManager.LoadScene(MainMenú);
    }

    
}