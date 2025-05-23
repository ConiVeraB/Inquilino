using UnityEngine;

public class LightZone : MonoBehaviour
{
    public Light directionalLight;

    public string playerTag = "Player";

    private bool hasBeenTriggeredThisFrame = false;

    void Start()
    {
        if (directionalLight == null)
        {
            Debug.LogError("Directional Light no asignada en el script LightToggleOnEnter en el objeto: " + gameObject.name);
            enabled = false; // Desactiva este script si no hay luz
            return;
        }

        // Asegurar que la luz comience apagada
        directionalLight.enabled = false;
        Debug.Log("Luz direccional iniciada APAGADA por script.");

    }

   void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (directionalLight != null)
            {
                // Cambia el estado de la luz (si está encendida la apaga, si está apagada la enciende)
                directionalLight.enabled = !directionalLight.enabled;

                if (directionalLight.enabled)
                {
                    Debug.Log(other.name + " entró en la zona. Luz direccional ENCENDIDA.");
                }
                else
                {
                    Debug.Log(other.name + " entró en la zona. Luz direccional APAGADA.");
                }
            }
        }
    }

 
}
