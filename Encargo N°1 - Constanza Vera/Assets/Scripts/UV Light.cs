using UnityEngine;

public class UVLight : MonoBehaviour
{
    public Light UV;
    public float maxDistance = 10f;
    public LayerMask interactableLayers;
    public float lerpSpeed = 10f;
    void Start()
    {
        UV.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UV.enabled = !UV.enabled;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 targetPosition;

        if (Physics.Raycast(ray, out hit, maxDistance, interactableLayers))
        {
            // El rayo golpeó algo en la capa interactuable.
            targetPosition = hit.point;
        }
        else
        {
            // El rayo no golpeó nada, apuntar a una distancia máxima.
            targetPosition = ray.origin + ray.direction * maxDistance;
        }

        // 2. Suavizar el movimiento (opcional, pero recomendable).
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);

        // 3. Mover la linterna.
        //transform.position = smoothedPosition; // Si quieres mover la linterna
        transform.LookAt(smoothedPosition); // Apuntar la linterna
    }
}

