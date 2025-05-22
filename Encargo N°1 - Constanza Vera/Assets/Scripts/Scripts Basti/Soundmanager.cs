using UnityEngine;
using UnityEngine.UI;

public class Soundmanager : MonoBehaviour
{
    public Slider VolumeSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("soundVolume"))
            loadVolume();
        else
        {
            PlayerPrefs.SetFloat("soundVolume", 1);
            loadVolume();
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
    }

}
