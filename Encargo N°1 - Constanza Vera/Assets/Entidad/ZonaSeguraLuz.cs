using UnityEngine;

public class ZonaSeguraLuz : MonoBehaviour
{
    public float regeneracionPorSegundo = 10f;
    public Light luzReferenciada;

    private bool jugadorDentro = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorDentro = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorDentro = false;
    }

    private void Update()
    {
        if (!jugadorDentro || CorduraManager.instancia == null)
            return;

        if (luzReferenciada != null && luzReferenciada.enabled)
        {
            CorduraManager.instancia.RecuperarCordura(regeneracionPorSegundo * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}


