using UnityEngine;
using System.Collections;

public class ApagadorDeLuz : MonoBehaviour
{
    [Header("Referencia a la luz que se apagará")]
    public Light luzAControlar;

    [Header("Configuración")]
    public bool apagarParaSiempre = true;
    public float tiempoDeApagado = 1.5f;

    private bool yaApagada = false;

    private void OnTriggerEnter(Collider other)
    {
        if (yaApagada) return;

        if (other.CompareTag("Enemy")) // Asegúrate de que El Sujeto tenga este tag
        {
            if (luzAControlar != null)
            {
                StartCoroutine(ApagarGradualmente(luzAControlar));
                yaApagada = apagarParaSiempre;
                Debug.Log("Luz apagada por El Sujeto: " + gameObject.name);
            }
        }
    }

    IEnumerator ApagarGradualmente(Light luz)
    {
        float intensidadOriginal = luz.intensity;
        float t = 0f;

        while (t < 1f)
        {
            luz.intensity = Mathf.Lerp(intensidadOriginal, 0f, t);
            t += Time.deltaTime / tiempoDeApagado;
            yield return null;
        }

        luz.intensity = 0f;
        luz.enabled = false;
    }
}
