using UnityEngine;
using UnityEngine.UI;

public class SwitchSpriteChanger : MonoBehaviour
{
    public Toggle toggle;                 // Asigna el Toggle
    public Image switchImage;            // La imagen que se cambia
    public Sprite switchOnSprite;        // Imagen encendido
    public Sprite switchOffSprite;       // Imagen apagado
    public ControladorLucesGrupo controladorLuces; 


    void Start()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(OnToggleChanged);
            OnToggleChanged(toggle.isOn); // Setear estado inicial
        }
    }

    public void OnToggleChanged(bool isOn)
    {
        if (switchImage != null)
        {
            switchImage.sprite = isOn ? switchOnSprite : switchOffSprite;
        }

        if (controladorLuces != null)
        {
            controladorLuces.EncenderLuces(isOn); //  activa/apaga luces reales
        }
    }

}
