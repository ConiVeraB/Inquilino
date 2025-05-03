using UnityEngine;

public class ControladorLucesGrupo : MonoBehaviour
{
    public Light[] luces; // Las luces individuales

    public void EncenderLuces(bool estado)
    {
        foreach (Light luz in luces)
        {
            if (luz != null)
                luz.enabled = estado;
        }
    }
}
