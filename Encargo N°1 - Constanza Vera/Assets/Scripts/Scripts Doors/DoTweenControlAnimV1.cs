using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class DoTweenControlAnimV1 : MonoBehaviour
{
    public float openAngle, closeAngle;
    bool isOpen;
    bool isAnimating = false;
    public Ease curveAnimation;

    // --- NUEVAS VARIABLES ---
    [Header("Auto Cierre")]
    [Tooltip("Tiempo en segundos que la puerta permanecerá abierta antes de cerrarse sola.")]
    public float autoCloseDelay = 5f;

    private Coroutine autoCloseCoroutine; // Para controlar nuestro temporizador

    [Header("Sonidos")]
    public AudioSource puerta;
    public AudioClip abrirPuertas;
    public AudioClip cerrarPuertas;

    // El Start y Update no son necesarios, los podemos quitar para limpiar.

    public void SetOpenDoor()
    {
        if (isAnimating) return;
        isAnimating = true;

        isOpen = !isOpen;
        switch (isOpen)
        {
            case true:
                // --- ABRIR LA PUERTA ---
                transform.DOLocalRotate(new Vector3(0, openAngle, 0), 1f, RotateMode.Fast)
                         .SetEase(curveAnimation)
                         .OnComplete(() => isAnimating = false);
                ReproducirSonido(abrirPuertas);

                // --- INICIAMOS EL TEMPORIZADOR PARA CERRAR ---
                // Si ya había un temporizador, lo detenemos para empezar uno nuevo.
                if (autoCloseCoroutine != null)
                {
                    StopCoroutine(autoCloseCoroutine);
                }
                autoCloseCoroutine = StartCoroutine(AutoCloseRoutine());
                break;

            case false:
                // --- CERRAR LA PUERTA (MANUALMENTE) ---
                // --- DETENEMOS EL TEMPORIZADOR POR SI ESTABA ACTIVO ---
                if (autoCloseCoroutine != null)
                {
                    StopCoroutine(autoCloseCoroutine);
                    autoCloseCoroutine = null; // Limpiamos la referencia
                }

                // Llamamos a la lógica de cierre.
                ForceCloseDoor();
                break;
        }
    }

    // --- NUEVA CORRUTINA / TEMPORIZADOR ---
    private IEnumerator AutoCloseRoutine()
    {
        // Espera el tiempo definido en autoCloseDelay
        yield return new WaitForSeconds(autoCloseDelay);

        Debug.Log("Temporizador finalizado. Forzando cierre de la puerta.");

        // Una vez esperado el tiempo, cierra la puerta.
        ForceCloseDoor();
    }

    // --- NUEVA FUNCIÓN PARA EVITAR REPETIR CÓDIGO ---
    // Esta función contiene la lógica para cerrar la puerta y puede ser llamada
    // tanto manualmente como por el temporizador.
    private void ForceCloseDoor()
    {
        // Solo cierra si está abierta y no se está animando
        if (!isOpen || isAnimating) return;

        isAnimating = true;
        isOpen = false; // Nos aseguramos de que el estado sea "cerrado"

        transform.DOLocalRotate(new Vector3(0, closeAngle, 0), 1f, RotateMode.Fast)
                 .SetEase(curveAnimation)
                 .OnComplete(() => isAnimating = false);

        ReproducirSonido(cerrarPuertas);
    }

    private void ReproducirSonido(AudioClip clip)
    {
        if (puerta != null && clip != null)
        {
            puerta.PlayOneShot(clip);
        }
    }
}