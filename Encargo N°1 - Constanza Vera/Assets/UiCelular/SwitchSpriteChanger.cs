using UnityEngine;
using UnityEngine.UI;

public class SwitchSpriteChanger : MonoBehaviour
{
    public Toggle toggle;                 
    public Image switchImage;            
    public Sprite switchOnSprite;        
    public Sprite switchOffSprite;       
    public ControladorLucesGrupo controladorLuces; 

    public AudioSource audioSource;
    public AudioClip touch;


    void Start()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(OnToggleChanged);
            OnToggleChanged(toggle.isOn); 
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
            controladorLuces.EncenderLuces(isOn); 
        }
        PlayButtonSound();
    }

    public void PlayButtonSound()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }

}
