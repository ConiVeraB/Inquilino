using UnityEngine;

public class Footstepcontroller : MonoBehaviour
{
    public AudioClip footstepClip;         
    public AudioSource footSource;

    [Header("Paso configuraciones")]
    public float stepInterval = 0.45f;     
    public float minPitch = 0.88f;
    public float maxPitch = 1.12f;
    public float movementThreshold = 0.12f;

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
            
            stepTimer = Mathf.Min(stepTimer, stepInterval);
        }

        lastPosition = currentPosition;
    }

    void PlayFootstep()
    {
        if (footstepClip == null) return;

        footSource.pitch = Random.Range(minPitch, maxPitch);
        footSource.volume = Random.Range(0.8f, 1f);
        footSource.PlayOneShot(footstepClip);
    }
}
