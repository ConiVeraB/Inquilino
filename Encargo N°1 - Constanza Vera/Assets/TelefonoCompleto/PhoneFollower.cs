using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("Target")] // Ayuda a organizar en el Inspector
    public Transform playerTransform; // Arrastra el Transform del jugador aquí

    [Header("Offsets")]
    public Vector3 positionOffset;    // Ajusta la posición relativa al jugador
    public Vector3 rotationOffset;    // Ajusta la rotación relativa al jugador

    [Header("Appearance")]
    public Vector3 desiredScale = new Vector3(0.1f, 0.1f, 0.1f);    // <-- ¡NUEVO! Añade esta línea (Vector3.one es (1, 1, 1) por defecto)

    public bool followEnabled = true; // Activado por defecto


    void LateUpdate()
    {
        if (!followEnabled || playerTransform == null)
            return;

        if (playerTransform == null)
        {
            Debug.LogWarning("Player Transform no asignado en el script FollowPlayer en el objeto: " + gameObject.name);
            return; // Salir si no hay jugador asignado para evitar errores
        }

        // --- Cálculo de Posición ---
        // Se calcula usando la posición y orientación actual del jugador más el offset
        Vector3 targetPosition = playerTransform.position +
                                 playerTransform.right * positionOffset.x +   // Mover a la derecha/izquierda del jugador
                                 playerTransform.up * positionOffset.y +      // Mover arriba/abajo del jugador
                                 playerTransform.forward * positionOffset.z; // Mover delante/detrás del jugador

        // --- Cálculo de Rotación ---
        // Se calcula rotando desde la orientación del jugador según el offset de rotación
        Quaternion targetRotation = playerTransform.rotation * Quaternion.Euler(rotationOffset);

        // --- Aplicar Transformaciones ---
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        //transform.localScale = desiredScale; // <-- ¡NUEVO! Aplicamos la escala deseada cada frame
    }

    void OnEnable()
    {
        transform.localScale = desiredScale; // una sola vez, no cada frame
    }

}



