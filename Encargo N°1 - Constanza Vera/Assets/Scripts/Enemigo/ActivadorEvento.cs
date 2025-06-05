using UnityEngine;

public class ActivadorEvento : MonoBehaviour
{
    public EnemyController sujeto;

    public bool aparecer = true;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || sujeto == null) return;

        if (aparecer)
            sujeto.ActivarSujeto();
        else
            sujeto.DesactivarSujeto();
    }
}
