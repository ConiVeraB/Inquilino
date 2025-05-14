using UnityEngine;

public class Footstepcontroller : MonoBehaviour
{
    public AudioClip[] footSteps;
    public AudioSource footSource;
    public float stepInterval = 0.5f;
    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;
    public float movementThreshold = 0.01f; // Para evitar pasos por movimientos mínimos

    private float stepTimer = 0f;
    private Vector3 lastPosition;

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
        float distanceMoved = Vector3.Distance(currentPosition, lastPosition);

        if (distanceMoved > movementThreshold)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }

        lastPosition = currentPosition;
    }

    void PlayFootstep()
    {
        if (footSteps.Length == 0) return;

        int randomClip = Random.Range(0, footSteps.Length);
        footSource.clip = footSteps[randomClip];
        footSource.pitch = Random.Range(minPitch, maxPitch);
        footSource.Play();
    } 
}
