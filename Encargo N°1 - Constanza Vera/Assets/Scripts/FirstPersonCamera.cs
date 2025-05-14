using UnityEngine;
using System; 


public class FirstPersonCamera : MonoBehaviour
{
    [Header("Camera")]
    public Transform cameraHolder; // asigna aquí el objeto "CameraHolder"

    [Header("Sensitivity")]
    public float mouseSensitivity = 2.5f;
    public float smoothing = 5f;
    public float verticalSensitivityFactor = 1.2f;


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

        // Ajusta la sensibilidad en el Inspector
        mouseInput *= mouseSensitivity;

        // Suavizado interpolado
        smoothedVelocity = Vector2.Lerp(smoothedVelocity, mouseInput, 1f / smoothing);

        // Acumulamos el movimiento suavizado
        currentLookingPos += smoothedVelocity;

        // Clampeamos solo el eje vertical
        currentLookingPos.y = Mathf.Clamp(currentLookingPos.y, -90f, 90f);

        // Aplicar rotaciones
        cameraHolder.localRotation = Quaternion.Euler(-currentLookingPos.y, 0, 0); // vertical
        transform.rotation = Quaternion.Euler(0, currentLookingPos.x, 0);         // horizontal
    }



}



