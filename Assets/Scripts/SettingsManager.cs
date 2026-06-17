using UnityEngine;

// Designed to handle logic for player pref loading, such as volume, sensitivity, and fullscreen mode.
public class SettingsManager : MonoBehaviour
{
    // Singleton instance for safeguarding
    public static SettingsManager Instance { get; private set; }

    public float lookSensitivity = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1f);
        lookSensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
        Screen.fullScreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
    }

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
