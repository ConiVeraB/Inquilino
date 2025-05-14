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

    [Header("Touch")]
    public AudioSource audioSource;
    public AudioClip touch;

    void Start()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(CambiarEstado);
            CambiarEstado(toggle.isOn);
        }
    }

    public void CambiarEstado(bool encendido)
    {
        
        if (switchImage != null)
        {
            switchImage.sprite = encendido ? switchOnSprite : switchOffSprite;
            PlayButtonSound();
        }

        
        foreach (Light luz in luces)
        {
            if (luz != null)
                luz.enabled = encendido;
            PlayButtonSound();
        }
    }

    public void PlayButtonSound()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }
}

