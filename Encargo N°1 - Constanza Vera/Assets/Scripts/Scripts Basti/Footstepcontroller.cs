using UnityEngine;

public class Footstepcontroller : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip footstepClip;
    public AudioSource footSource;

    [Header("Configuración de pasos")]
    public float stepDistance = 1.5f;     // Distancia que debe caminar antes de sonar un paso
    public float minPitch = 0.88f;        // Rango aleatorio de pitch
    public float maxPitch = 1.12f;
    public float minVolume = 0.9f;        // Rango aleatorio de volumen
    public float maxVolume = 1f;
    public float movementThreshold = 0.01f; // Umbral mínimo de movimiento por frame

    private Vector3 lastPosition;
    private float accumulatedDistance = 0f;

    void Start()
    {
        lastPosition = transform.position;

        if (footSource == null)
        {
            footSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        float frameDistance = Vector3.Distance(currentPosition, lastPosition);

        // Solo acumula si realmente se movió
        if (frameDistance > movementThreshold)
        {
            accumulatedDistance += frameDistance;

            if (accumulatedDistance >= stepDistance)
            {
                PlayFootstep();
                accumulatedDistance = 0f;
            }
        }

        lastPosition = currentPosition;
    }

    void PlayFootstep()
    {
        if (footstepClip == null || footSource == null) return;

        footSource.pitch = Random.Range(minPitch, maxPitch);
        footSource.volume = Random.Range(minVolume, maxVolume);
        footSource.PlayOneShot(footstepClip);
    }
}
