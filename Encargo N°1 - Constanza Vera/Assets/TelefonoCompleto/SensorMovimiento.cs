using UnityEngine;

public class SensorMovimiento : MonoBehaviour
{
    public string nombreZona = "Zona sin nombre";
    public PhoneController phoneController;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Movimiento detectado en: " + nombreZona);

            if (phoneController != null)
            {
                phoneController.MostrarNotificacionDesdeSensor(nombreZona);
            }

        }

    }
}

