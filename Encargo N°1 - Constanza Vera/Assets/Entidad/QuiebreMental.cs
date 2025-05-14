using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuiebreMental : MonoBehaviour
{
    [Header("Activaciones únicas por umbral")]
    public bool evento70Activado = false;
    public bool evento40Activado = false;
    public bool evento15Activado = false;

    [Header("Flash visual perturbador")]
    public GameObject flashVisual;              // Imagen blanca en un Canvas Overlay
    public float duracionFlash = 0.5f;

    [Header("Voces internas")]
    public AudioClip voz70;
    public AudioClip voz40;
    public AudioClip voz15;

    [Header("Control del jugador")]
    public MonoBehaviour movimientoJugador;     // Ej: PlayerMovement
    public MonoBehaviour camaraJugador;         // Ej: FirstPersonLook

    void Update()
    {
        if (CorduraManager.instancia == null) return;

        float nivel = CorduraManager.instancia.cordura;

        if (nivel <= 70 && !evento70Activado)
        {
            evento70Activado = true;
            ActivarEvento(voz70);
        }

        if (nivel <= 40 && !evento40Activado)
        {
            evento40Activado = true;
            ActivarEvento(voz40);
        }

        if (nivel <= 15 && !evento15Activado)
        {
            evento15Activado = true;
            ActivarColapsoMental();
        }
    }

    void ActivarEvento(AudioClip clip)
    {
        if (clip != null && CorduraManager.instancia.fuenteSonidosInternos != null)
            CorduraManager.instancia.fuenteSonidosInternos.PlayOneShot(clip);

        if (flashVisual != null)
            StartCoroutine(FlashPerturbador());
    }

    void ActivarColapsoMental()
    {
        ActivarEvento(voz15);

        if (movimientoJugador != null)
            movimientoJugador.enabled = false;

        if (camaraJugador != null)
            camaraJugador.enabled = false;

        Invoke(nameof(RestaurarControl), 4f);
    }

    void RestaurarControl()
    {
        if (movimientoJugador != null)
            movimientoJugador.enabled = true;

        if (camaraJugador != null)
            camaraJugador.enabled = true;
    }

    IEnumerator FlashPerturbador()
    {
        flashVisual.SetActive(true);
        Image img = flashVisual.GetComponent<Image>();

        for (int i = 0; i < 4; i++)
        {
            img.color = new Color(1f, 1f, 1f, Random.Range(0.3f, 0.8f));
            yield return new WaitForSeconds(0.05f);
            img.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(0.05f);
        }

        flashVisual.SetActive(false);
    }
}
