using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public Light lightToControl;
    private bool isLightOn = false;
    private bool playerInRange = false;

    void Start()
    {
        // Asegurarse de que la luz empiece apagada
        lightToControl.enabled = false;
    }
    void Update()
    {
        // Verifica si el jugador está cerca y presiona la tecla E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleLight(); // Cambia el estado de la luz
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el jugador entra en el área
        {
            playerInRange = true; // El jugador está cerca
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si el jugador sale del área
        {
            playerInRange = false; // El jugador ya no está cerca
        }
    }

    void ToggleLight()
    {
        isLightOn = !isLightOn;

        if (isLightOn)
        {
            lightToControl.enabled = true; // Enciende la luz
        }
        else
        {
            lightToControl.enabled = false; // Apaga la luz
        }
    }
}


