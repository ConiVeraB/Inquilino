using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyPatrol patrulla;

    private void Awake()
    {
        if (patrulla == null) patrulla = GetComponent<EnemyPatrol>();
    }

    public void ActivarSujeto()
    {
        gameObject.SetActive(true);
        patrulla.IniciarPatrulla();
        Debug.Log("El Sujeto ha aparecido.");
    }

    public void DesactivarSujeto()
    {
        patrulla.DetenerPatrulla();
        gameObject.SetActive(false);
        Debug.Log("El Sujeto ha desaparecido.");
    }
}


