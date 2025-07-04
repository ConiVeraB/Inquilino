using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class PhotoQuestManager : MonoBehaviour
{
    [Header("Configuración de la Misión")]
    [Tooltip("El número total de fotos que el jugador puede tomar.")]
    public int maxPhotos = 4;
    [Tooltip("El número mínimo de fotos correctas para ganar.")]
    public int photosToWin = 2;

    [Header("Escenas")]
    [Tooltip("El nombre EXACTO de la escena a cargar si el jugador gana.")]
    public string winSceneName;
    [Tooltip("El nombre EXACTO de la escena a cargar si el jugador pierde.")]
    public string loseSceneName;

    // Contadores internos
    private int totalPhotosTaken = 0;
    private int correctPhotosCount = 0;

    // Para evitar que se pueda seguir jugando después de terminar
    private bool isQuestActive = true;

    // Esta función será llamada por el script 'Photos'
    public void RegisterPhoto(bool wasCorrect)
    {
        // Si la misión ya no está activa, no hacemos nada.
        if (!isQuestActive) return;

        totalPhotosTaken++;

        if (wasCorrect)
        {
            correctPhotosCount++;
        }

        Debug.Log("Misión: Foto " + totalPhotosTaken + "/" + maxPhotos + ". Fotos correctas: " + correctPhotosCount);

        // Comprobamos si se ha llegado al final de la misión
        if (totalPhotosTaken >= maxPhotos)
        {
            EndQuest();
        }
    }

    // Función pública para que el script Photos sepa si puede tomar fotos
    public bool IsQuestActive()
    {
        return isQuestActive;
    }

    private void EndQuest()
    {
        isQuestActive = false; // La misión ha terminado.
        Debug.Log("¡Fin de la misión! Evaluando resultado...");

        // Caso 2: El jugador gana si tiene 2, 3 o 4 fotos correctas.
        if (correctPhotosCount >= photosToWin)
        {
            Debug.Log("¡VICTORIA! El jugador ha conseguido " + correctPhotosCount + " fotos correctas.");
            SceneManager.LoadScene(winSceneName);
        }
        // Caso 1: El jugador pierde si tiene 0 o 1 foto correcta.
        else
        {
            Debug.Log("DERROTA. El jugador solo consiguió " + correctPhotosCount + " fotos correctas.");
            SceneManager.LoadScene(loseSceneName);
        }
    }
}
