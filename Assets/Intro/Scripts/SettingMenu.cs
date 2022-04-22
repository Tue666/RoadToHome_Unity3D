using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class SettingMenu : MonoBehaviour
{
    public Dropdown resolutionDropdown;

    public Toggle fullscreenToogle;

    public Dropdown qualitydropdown;

    Resolution[] resolutions;

    private bool isFullScreen = false;

    const string PrefName = "optionvalue";

    const string resName = "resolutionoption";

    void Awake()
    {
        if (PlayerPrefs.GetInt("togglestate") == 1)
        {
            isFullScreen = true;
            fullscreenToogle.isOn = true;
        }
        else
            fullscreenToogle.isOn = false;

        resolutionDropdown.onValueChanged.AddListener(new UnityAction<int>(index =>
        {
            PlayerPrefs.SetInt(resName, resolutionDropdown.value);
            PlayerPrefs.Save();
        }));

        qualitydropdown.onValueChanged.AddListener(new UnityAction<int>(index =>
        {
            PlayerPrefs.SetInt(PrefName, qualitydropdown.value);
            PlayerPrefs.Save();
        }));

    }
    void Start()
    {
        qualitydropdown.value = PlayerPrefs.GetInt(PrefName, 6);
        
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;

        List<string> options = new List<string>();

        for (int i = 0; i< resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "hz";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height
                && resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt(resName, currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();

    }

    public void SetResolution(int resolutionIndex)
    {

        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        if (isFullScreen == false)
        {
            PlayerPrefs.SetInt("togglestate", 0);
        }
        else
        {
            isFullScreen = true;
            PlayerPrefs.SetInt("togglestate", 1);
        }
            
    }

}
