using UnityEngine;

public class PhoneCameraManager : MonoBehaviour
{
    public Camera cameraCelular;
    public GameObject panelCamara;

    void Update()
    {
        if (cameraCelular == null || panelCamara == null)
            return;

        // Activar o desactivar la Camera_Celular según el estado del PanelCamara
        if (panelCamara.activeInHierarchy)
        {
            if (!cameraCelular.enabled)
            {
                cameraCelular.enabled = true;
            }
        }
        else
        {
            if (cameraCelular.enabled)
            {
                cameraCelular.enabled = false;
            }
        }
    }
}
