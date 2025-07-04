using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyEventController : MonoBehaviour
{
    // --- Variables del Evento ---
    [Header("Configuración del Enemigo")]
    public GameObject enemyObject;
    public Transform enemySpawnPoint;
    public float enemyVisibleDuration = 10f;
    public Animator enemyAnimator;

    // --- Variables de Patrullaje (con la nueva variable de detección) ---
    [Header("Configuración de Patrullaje")]
    [Tooltip("Velocidad de movimiento del enemigo al patrullar.")]
    public float patrolSpeed = 1.0f;
    [Tooltip("Velocidad de rotación.")]
    public float rotationSpeed = 0.5f;
    [Tooltip("Tiempo que el enemigo esperará antes de cambiar de rutina.")]
    public float timeBetweenRoutines = 4.0f;
    [Tooltip("Distancia hacia adelante para detectar paredes.")]
    public float obstacleCheckDistance = 0.5f; // Variable para el Raycast

    // --- Variables de Configuración del Evento (Sin cambios) ---
    [Header("Configuración de Iluminación")]
    public List<Light> lightsToAffect;
    private List<float> originalLightIntensities = new List<float>();

    [Header("Configuración de Audio")]
    public AudioSource ambientAudioSource;
    public AudioSource jumpscareAudioSource;

    [Header("Tiempos del Evento")]
    public float preSpawnDelay = 1f;
    public float lightsOffDuration = 12f;

    [Header("Encadenamiento de Eventos (Opcional)")]
    public GameObject nextTriggerZone;

    private bool eventActive = false;
    private Coroutine patrolCoroutine;

    void Awake()
    {
        // El Awake no necesita cambios.
        foreach (Light light in lightsToAffect) { originalLightIntensities.Add(light.intensity); }
        if (enemyObject != null) { enemyObject.SetActive(false); }
        if (nextTriggerZone != null) { nextTriggerZone.SetActive(false); }
    }

    public void StartEvent()
    {
        if (eventActive) return;
        eventActive = true;
        StartCoroutine(EventSequence());
    }

    // La corrutina principal del evento. Inicia y detiene el patrullaje.
    private IEnumerator EventSequence()
    {
        Debug.Log("Evento del enemigo iniciado.");
        foreach (Light light in lightsToAffect) { light.intensity = 0f; }
        if (ambientAudioSource != null) { ambientAudioSource.Play(); }
        yield return new WaitForSeconds(preSpawnDelay);

        // Inicia el patrullaje
        if (enemyObject != null)
        {
            enemyObject.transform.position = enemySpawnPoint.position;
            enemyObject.transform.rotation = enemySpawnPoint.rotation;
            enemyObject.SetActive(true);
            patrolCoroutine = StartCoroutine(PatrolRoutineAdapted());
            Debug.Log("Enemigo apareció y ha comenzado a patrullar.");
        }

        if (jumpscareAudioSource != null) { jumpscareAudioSource.Play(); }
        yield return new WaitForSeconds(enemyVisibleDuration);

        // Detiene el patrullaje
        if (enemyObject != null)
        {
            if (patrolCoroutine != null)
            {
                StopCoroutine(patrolCoroutine);
                patrolCoroutine = null;
            }
            if (enemyAnimator != null) { enemyAnimator.SetBool("Caminar", false); }
            enemyObject.SetActive(false);
            Debug.Log("Enemigo desapareció.");
        }

        // Restaura el resto del escenario
        float remainingLightsOffTime = lightsOffDuration - (preSpawnDelay + enemyVisibleDuration);
        if (remainingLightsOffTime > 0) { yield return new WaitForSeconds(remainingLightsOffTime); }
        for (int i = 0; i < lightsToAffect.Count; i++) { if (lightsToAffect[i] != null) { lightsToAffect[i].intensity = originalLightIntensities[i]; } }
        if (ambientAudioSource != null) { ambientAudioSource.Stop(); }
        if (jumpscareAudioSource != null) { jumpscareAudioSource.Stop(); }
        if (nextTriggerZone != null) { nextTriggerZone.SetActive(true); }
        eventActive = false;
        Debug.Log("Evento del enemigo completado.");
    }

    // --- CORRUTINA DE PATRULLAJE CON DETECCIÓN DE OBSTÁCULOS ---
    private IEnumerator PatrolRoutineAdapted()
    {
        while (true) // Bucle infinito que se ejecuta hasta que lo detiene EventSequence
        {
            // === ETAPA 1: ESPERAR ===
            if (enemyAnimator != null) enemyAnimator.SetBool("Caminar", false);
            Debug.Log("Patrulla: Esperando...");
            yield return new WaitForSeconds(timeBetweenRoutines);

            // === ETAPA 2: GIRAR Y CAMINAR ===
            Debug.Log("Patrulla: Decidiendo nueva dirección y caminando...");

            float grade = Random.Range(0, 360);
            Quaternion targetAngle = Quaternion.Euler(0, grade, 0);

            float timer = 0;
            if (enemyAnimator != null) enemyAnimator.SetBool("Caminar", true);

            while (timer < timeBetweenRoutines)
            {
                // --- LÓGICA DE DETECCIÓN DE OBSTÁCULOS ---
                RaycastHit hit;
                // Dibuja el rayo en la vista de escena para que podamos verlo.
                Debug.DrawRay(enemyObject.transform.position, enemyObject.transform.forward * obstacleCheckDistance, Color.red);

                // Lanza el rayo. Si choca con algo...
                if (Physics.Raycast(enemyObject.transform.position, enemyObject.transform.forward, out hit, obstacleCheckDistance))
                {
                    // ...y ese algo no es el propio enemigo...
                    if (hit.transform != enemyObject.transform)
                    {
                        Debug.Log("Patrulla: ¡Pared detectada! Forzando cambio de rutina.");
                        // Rompe el bucle de caminar para pasar a la siguiente etapa (esperar y elegir nueva dirección).
                        break;
                    }
                }
                // --- FIN DE LA LÓGICA DE DETECCIÓN ---

                // Si no hay obstáculo, procede con el movimiento y la rotación.
                enemyObject.transform.rotation = Quaternion.RotateTowards(enemyObject.transform.rotation, targetAngle, rotationSpeed);
                enemyObject.transform.Translate(Vector3.forward * patrolSpeed * Time.deltaTime);

                timer += Time.deltaTime;
                yield return null; // Espera al siguiente frame
            }
        }
    }
}
