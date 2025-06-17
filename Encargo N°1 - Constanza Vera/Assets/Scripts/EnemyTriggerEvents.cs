using UnityEngine;

public class EnemyTriggerEvents : MonoBehaviour
{
    public EnemyEventController eventController;

    private void OnTriggerEnter(Collider other)
    {
        // Asegúrate de que solo el jugador (con el tag "Player") active el evento.
        // Asegúrate de que tu jugador tenga un Rigidbody y un Collider.
        if (other.CompareTag("Player"))
        {
            if (eventController != null)
            {
                eventController.StartEvent();
                // Desactiva este trigger para que no se active múltiples veces.
                // Si quieres que pueda activarse de nuevo, puedes quitar esta línea.
                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("EnemyEventController no asignado en " + gameObject.name + "!");
            }
        }
    }
}

