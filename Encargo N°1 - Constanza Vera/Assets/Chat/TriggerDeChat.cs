using UnityEngine;

public class TriggerDeChat : MonoBehaviour
{
    public ChatSecuencialDecisiones chatAsociado;

    public void ActivarChat()
    {
        if (chatAsociado == null)
        {
            Debug.LogWarning($"[{name}] No hay chatAsociado asignado.");
            return;
        }

    }
}

