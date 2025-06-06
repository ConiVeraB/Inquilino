using UnityEngine;

public class TriggerNotificacion : MonoBehaviour
{
    public NotificacionInicioUI notificacionUI;

    private bool yaActivado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (yaActivado) return;

        if (other.CompareTag("Player"))
        {
            notificacionUI?.MostrarNotificacion();
            yaActivado = true;
        }
    }
}

