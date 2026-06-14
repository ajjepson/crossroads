using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Slider volumeSlider;
    //public Slider othersSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.HasKey("soundVolume"))
        {
            loadVolume();
        }
        else
        {
            PlayerPrefs.SetFloat("soundVolume", 1);
            loadVolume();
        }
    }

    public void controlVolume()
    {
        AudioListener.volume = volumeSlider.value;
        keepVolume();
    }

    public void keepVolume()
    {
        PlayerPrefs.SetFloat("soundVolume", volumeSlider.value);
    }
    public void loadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("soundVolume");
    }
}
