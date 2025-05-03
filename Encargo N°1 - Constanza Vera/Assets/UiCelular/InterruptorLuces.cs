using UnityEngine;
using UnityEngine.UI;

public class InterruptorLuces : MonoBehaviour
{
    [Header("Referencias")]
    public Toggle toggle;
    public Image switchImage;
    public Sprite switchOnSprite;
    public Sprite switchOffSprite;
    public Light[] luces;

    void Start()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(CambiarEstado);
            CambiarEstado(toggle.isOn); // Aplica el estado inicial
        }
    }

    public void CambiarEstado(bool encendido)
    {
        // Cambiar sprite visual del switch
        if (switchImage != null)
        {
            switchImage.sprite = encendido ? switchOnSprite : switchOffSprite;
        }

        // Encender/apagar luces reales
        foreach (Light luz in luces)
        {
            if (luz != null)
                luz.enabled = encendido;
        }
    }
}

