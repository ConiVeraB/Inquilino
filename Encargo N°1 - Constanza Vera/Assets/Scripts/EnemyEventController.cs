using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyEventController : MonoBehaviour
{
    [Header("Configuración del Enemigo")]
    [Tooltip("El GameObject del enemigo que aparecerá.")]
    public GameObject enemyObject;
    [Tooltip("La posición donde aparecerá el enemigo (si no es su posición inicial).")]
    public Transform enemySpawnPoint;
    [Tooltip("Duración en segundos que el enemigo será visible.")]
    public float enemyVisibleDuration = 5f;

    [Header("Configuración de Iluminación")]
    [Tooltip("Todas las luces que se apagarán durante el evento.")]
    public List<Light> lightsToAffect;
    private List<float> originalLightIntensities = new List<float>(); // Para guardar las intensidades originales

    [Header("Configuración de Audio")]
    [Tooltip("Audio Source para el sonido ambiental de miedo.")]
    public AudioSource ambientAudioSource;
    [Tooltip("Audio Source para el 'jumpscare' o sonido repentino.")]
    public AudioSource jumpscareAudioSource;

    [Header("Tiempos del Evento")]
    [Tooltip("Retraso antes de que el enemigo aparezca después de que el trigger sea activado (opcional).")]
    public float preSpawnDelay = 1f;
    [Tooltip("Duración total en segundos que las luces permanecerán apagadas.")]
    public float lightsOffDuration = 7f; // Debe ser mayor o igual que enemyVisibleDuration + preSpawnDelay

    [Header("Encadenamiento de Eventos (Opcional)")]
    [Tooltip("El siguiente GameObject con EnemyEventTrigger que se activará después de que este evento termine.")]
    public GameObject nextTriggerZone;

    private bool eventActive = false; // Para evitar que el evento se inicie varias veces.

    void Awake()
    {
        // Guarda las intensidades originales de las luces al inicio.
        foreach (Light light in lightsToAffect)
        {
            originalLightIntensities.Add(light.intensity);
        }

        // Asegúrate de que el enemigo esté inactivo al inicio.
        if (enemyObject != null)
        {
            enemyObject.SetActive(false);
        }

        // Asegúrate de que la siguiente zona trigger esté inactiva si está asignada.
        if (nextTriggerZone != null)
        {
            nextTriggerZone.SetActive(false);
        }
    }

    public void StartEvent()
    {
        if (eventActive) return; // Si el evento ya está en curso, no hacer nada.
        eventActive = true;
        StartCoroutine(EventSequence());
    }

    private IEnumerator EventSequence()
    {
        Debug.Log("Evento del enemigo iniciado.");

        // --- 1. Apagar luces ---
        foreach (Light light in lightsToAffect)
        {
            light.intensity = 0f; // O light.enabled = false;
        }

        // --- 2. Reproducir audio ambiental ---
        if (ambientAudioSource != null && ambientAudioSource.clip != null)
        {
            ambientAudioSource.Play();
        }

        // --- 3. Retraso antes de la aparición del enemigo ---
        yield return new WaitForSeconds(preSpawnDelay);

        // --- 4. Aparecer enemigo ---
        if (enemyObject != null)
        {
            enemyObject.SetActive(true);
            if (enemySpawnPoint != null)
            {
                enemyObject.transform.position = enemySpawnPoint.position;
                enemyObject.transform.rotation = enemySpawnPoint.rotation;
            }
            Debug.Log("Enemigo apareció.");
        }

        // --- 5. Reproducir Jumpscare (si aplica) ---
        if (jumpscareAudioSource != null && jumpscareAudioSource.clip != null)
        {
            jumpscareAudioSource.Play();
        }

        // --- 6. Esperar que el enemigo esté visible ---
        yield return new WaitForSeconds(enemyVisibleDuration);

        // --- 7. Desaparecer enemigo ---
        if (enemyObject != null)
        {
            enemyObject.SetActive(false);
            Debug.Log("Enemigo desapareció.");
        }

        // --- 8. Esperar el resto del tiempo con las luces apagadas ---
        // Asegura que lightsOffDuration sea mayor que (preSpawnDelay + enemyVisibleDuration)
        float remainingLightsOffTime = lightsOffDuration - (preSpawnDelay + enemyVisibleDuration);
        if (remainingLightsOffTime > 0)
        {
            yield return new WaitForSeconds(remainingLightsOffTime);
        }

        // --- 9. Encender luces a su intensidad original ---
        for (int i = 0; i < lightsToAffect.Count; i++)
        {
            if (lightsToAffect[i] != null)
            {
                lightsToAffect[i].intensity = originalLightIntensities[i]; // O lightsToAffect[i].enabled = true;
            }
        }
        Debug.Log("Luces restauradas.");

        // --- 10. Detener audios ---
        if (ambientAudioSource != null) { ambientAudioSource.Stop(); }
        if (jumpscareAudioSource != null) { jumpscareAudioSource.Stop(); }

        // --- 11. Activar la siguiente zona trigger (si hay) ---
        if (nextTriggerZone != null)
        {
            nextTriggerZone.SetActive(true);
            Debug.Log("Siguiente zona trigger activada: " + nextTriggerZone.name);
        }

        eventActive = false; // El evento ha terminado, puede volver a iniciarse (si el trigger se reactiva).
        Debug.Log("Evento del enemigo completado.");
    }
}
