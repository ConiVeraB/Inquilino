using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ObjectInspection : MonoBehaviour
{
    [Header("Punto de inspección (frente a la cámara)")]
    public Transform inspectAnchor;

    [Header("Imagen flotante de interacción")]
    public GameObject interactionImage;

    [Header("Control del jugador")]
    public MonoBehaviour playerMovementScript;
    public FirstPersonCamera cameraLookScript;

    [Header("Lectura durante inspección")]
    public GameObject readPrompt;  // Texto "Presiona R para Leer"
    public GameObject readImage;   // Imagen con contenido de lectura
    
    [Header("Layer de enfoque sin post-procesado")]
    public string focusLayerName = "FocusedObject"; // debe existir en el proyecto

    private int originalLayer;
    public Volume postProcessVolume;

    [Header("Movimiento hacia el centro")]
    public float moveSpeed = 10f;

    private bool isInRange = false;
    private bool isInspecting = false;
    private bool isReading = false;
    private bool positionSnapped = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        if (interactionImage != null)
            interactionImage.SetActive(false);

        if (readPrompt != null)
            readPrompt.SetActive(false);

        if (readImage != null)
            readImage.SetActive(false);

        if (postProcessVolume != null)
            postProcessVolume.enabled = false;
    }

    void Update()
    {
        if (isInRange && !isInspecting && Input.GetKeyDown(KeyCode.E))
        {
            StartInspection();
            
        }
        else if (isInspecting && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
        {
            EndInspection();
        }

        if (isInspecting)
        {
            // Mover objeto hacia el punto de inspección
            if (!positionSnapped)
            {
                float distance = Vector3.Distance(transform.position, inspectAnchor.position);
                if (distance > 0.01f)
                {
                    transform.position = Vector3.Lerp(transform.position, inspectAnchor.position, Time.deltaTime * moveSpeed);
                }
                else
                {
                    transform.position = inspectAnchor.position;
                    positionSnapped = true;
                }
            }

            RotateObject();

            // Mostrar texto de lectura
            if (Input.GetKeyDown(KeyCode.R) && readImage != null)
            {
                isReading = !isReading;
                readImage.SetActive(isReading);
            }
        }
    }

    void StartInspection()
    {
        isInspecting = true;
        isInRange = false;
        positionSnapped = false;

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer(focusLayerName);
        cameraLookScript.LookObjectCamera.SetActive(true);  

        if (interactionImage != null)
            interactionImage.SetActive(false);

        if (readPrompt != null)
            readPrompt.SetActive(true);

        if (readImage != null)
            readImage.SetActive(false);

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        if (cameraLookScript != null)
            cameraLookScript.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (postProcessVolume != null)
        {
            postProcessVolume.enabled = true;  // Activa el efecto al comenzar inspección
        }
    }

    void EndInspection()
    {
        isInspecting = false;
        isReading = false;

        transform.position = originalPosition;
        transform.rotation = originalRotation;
        gameObject.layer = originalLayer;

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        if (cameraLookScript != null)
            cameraLookScript.enabled = true;

        if (readPrompt != null)
            readPrompt.SetActive(false);

        if (readImage != null)
            readImage.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraLookScript.LookObjectCamera.SetActive(false);

        if (isInRange && interactionImage != null)
            interactionImage.SetActive(true);

        if (postProcessVolume != null)
        {
            postProcessVolume.enabled = false;  // Desactiva el efecto al terminar inspección
        }
    }

    void RotateObject()
    {
        float rotX = Input.GetAxis("Mouse X") * 100 * Time.deltaTime;
        float rotY = Input.GetAxis("Mouse Y") * 100 * Time.deltaTime;

        transform.Rotate(Vector3.up, -rotX, Space.World);
        transform.Rotate(Vector3.right, rotY, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isInspecting)
        {
            isInRange = true;
            if (interactionImage != null)
                interactionImage.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            if (interactionImage != null)
                interactionImage.SetActive(false);
        }
    }
}

