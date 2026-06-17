using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject settingsMenuCanvas;

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Toggle fullscreenToggle;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggled);
    }

    void OnEnable()
    {
        // Load settings from PlayerPrefs or set default values
        volumeSlider.SetValueWithoutNotify(AudioListener.volume);
        sensitivitySlider.SetValueWithoutNotify(SettingsManager.Instance.lookSensitivity);
        fullscreenToggle.SetIsOnWithoutNotify(Screen.fullScreen);
    }


    void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
    }
    void OnSensitivityChanged(float value)
    {
        SettingsManager.Instance.lookSensitivity = value;
    }
    void OnFullscreenToggled(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void OnExit()
    {
        // Save settings to PlayerPrefs
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
        PlayerPrefs.SetFloat("Sensitivity", SettingsManager.Instance.lookSensitivity);
        PlayerPrefs.SetInt("Fullscreen", Screen.fullScreen ? 1 : 0);
        PlayerPrefs.Save();
        settingsMenuCanvas.SetActive(false);
    }
}
