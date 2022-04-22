using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class VolumeSliderController : MonoBehaviour
{
    public string VolumeType = "MasterVolume";
    public AudioMixer audioMixer;
    public Slider Slider;
    public float Multiplier = 30f;
    public Text volumeText; 
    [Range(0, 1)] public float DefaultSliderPercentage = 0.75f;

    void Awake()
    {
        if (!PlayerPrefs.HasKey(VolumeType))
        {
            PlayerPrefs.SetFloat(VolumeType, DefaultSliderPercentage);
        }
        Slider.onValueChanged.AddListener(SetSliderVolume);
        Slider.value = PlayerPrefs.GetFloat(VolumeType);
    }
    public void SetSliderVolume(float value)
    {
        audioMixer.SetFloat(VolumeType, SliderToDecibel(value));
        volumeText.text = Mathf.Round(value * 100f) + "%";
        PlayerPrefs.SetFloat(VolumeType, value);
        PlayerPrefs.Save();
    }
    private float SliderToDecibel(float value)
    {
        return Mathf.Clamp(Mathf.Log10(value/DefaultSliderPercentage) * Multiplier, -80f, 20f);
    }

}
