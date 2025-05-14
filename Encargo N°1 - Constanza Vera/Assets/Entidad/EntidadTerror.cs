using UnityEngine;

public class EntidadTerror : MonoBehaviour
{
    public float rangoDeteccion = 5f;
    public float dañoPorSegundo = 10f;
    public Transform jugador;

    void Update()
    {
        if (jugador == null || CorduraManager.instancia == null)
            return;

        float distancia = Vector3.Distance(jugador.position, transform.position);

        if (distancia <= rangoDeteccion)
        {
            CorduraManager.instancia.RestarCordura(dañoPorSegundo * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}

