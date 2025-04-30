using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Camera")]
    public Transform cameraHolder; // asigna aquí el objeto "CameraHolder"

    [Header("Sensitivity")]
    public float mouseSensitivity = 2.5f;
    public float smoothing = 5f;

    private Vector2 smoothedVelocity;
    private Vector2 currentLookingPos;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mouseInput = new Vector2(
            Input.GetAxisRaw("Mouse X"),
            Input.GetAxisRaw("Mouse Y")
        );

        mouseInput *= mouseSensitivity;

        smoothedVelocity = Vector2.Lerp(smoothedVelocity, mouseInput, 1f / smoothing);
        currentLookingPos += smoothedVelocity;

        currentLookingPos.y = Mathf.Clamp(currentLookingPos.y, -90f, 90f);

        // Rotación vertical (cámara)
        cameraHolder.localRotation = Quaternion.Euler(-currentLookingPos.y, 0, 0);

        // Rotación horizontal (jugador)
        transform.rotation = Quaternion.Euler(0, currentLookingPos.x, 0);
    }
}



