using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Soundmanager : MonoBehaviour
{
    public AudioMixer SFXmixer;
    public Slider VolumeSlider;
    public Slider SFXslider;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("soundVolume"))
            loadVolume();
        else
        {
            PlayerPrefs.SetFloat("soundVolume", 1);
            loadVolume();
            setVolume();
        }
    }
    public void setVolume()
    {
        AudioListener.volume = VolumeSlider.value;
    }

    public void saveVolume()
    {
        PlayerPrefs.SetFloat ("soundVolume", VolumeSlider.value);
    }

    public void loadVolume()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("soundVolume");
        SFXslider.value = PlayerPrefs.GetFloat("SFXVolume");

        SetSFXVolume();
    }

    public void SetSFXVolume()
    {
        float volume = SFXslider.value;
        SFXmixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
