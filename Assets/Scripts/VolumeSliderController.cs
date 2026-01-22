using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    [Header("Mixer Settings")]
    [Tooltip("The AudioMixer asset to control.")]
    [SerializeField] private AudioMixer targetMixer;

    [Tooltip("The name of the exposed volume parameter in the Mixer (e.g., 'MasterVolume').")]
    [SerializeField] private string exposedParamName = "MasterVolume";

    private Slider volumeSlider;

    private const float MIN_VOLUME_DB = -50f; // Silence
    private const float MAX_VOLUME_DB = 1f; // Full volume

    void Awake()
    {
        volumeSlider = GetComponent<Slider>();

        if (volumeSlider == null)
        {
            Debug.LogError("VolumeSliderController must be attached to a Slider component.");
            return;
        }

        // Add a listener to the slider: when its value changes, call the SetVolume method
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // Load and set the initial volume value (optional: persistence)
        LoadInitialVolume();
    }

    /// <summary>
    /// Converts the slider's 0-1 value to the Mixer's dB value and sets it.
    /// </summary>
    public void SetVolume(float normalizedValue)
    {
        // 1. Convert the 0-1 normalized value to a logarithmic (dB) scale.
        //    The formula: 20 * log10(value). We clamp the minimum to prevent log(0).
        float dbVolume = Mathf.Lerp(MIN_VOLUME_DB, MAX_VOLUME_DB, normalizedValue);

        // 2. Set the volume on the exposed parameter in the Audio Mixer.
        if (targetMixer != null)
        {
            // Note: If you want true silence, you usually set the value below 0.0001 or check if value == 0
            if (normalizedValue == 0)
            {
                targetMixer.SetFloat(exposedParamName, MIN_VOLUME_DB); // Ensure absolute silence
            }
            else
            {
                targetMixer.SetFloat(exposedParamName, dbVolume);
            }

            // Optional: Save the volume preference
            PlayerPrefs.SetFloat(exposedParamName, normalizedValue);
        }
    }

    private void LoadInitialVolume()
    {
        // Check if a volume preference was saved
        if (PlayerPrefs.HasKey(exposedParamName))
        {
            float savedValue = PlayerPrefs.GetFloat(exposedParamName);
            volumeSlider.value = savedValue;

            // Set the mixer volume immediately when starting
            SetVolume(savedValue);
        }
        else
        {
            // If no preference is saved, use the slider's default value (usually 1.0)
            SetVolume(volumeSlider.value);
        }
    }
}