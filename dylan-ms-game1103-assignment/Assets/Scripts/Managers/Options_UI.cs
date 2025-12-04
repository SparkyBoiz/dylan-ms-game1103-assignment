using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [Header("Volume Sliders")]
    [SerializeField] private Slider _mainVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _voiceVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

    private void Start()
    {
        // Check if the AudioManager instance exists before proceeding.
        if (Manager_Audio.Instance != null)
        {
            // Set the initial values of the sliders to match the current volume settings.
            _mainVolumeSlider.value = Manager_Audio.Instance.MainVolume;
            _musicVolumeSlider.value = Manager_Audio.Instance.MusicVolume;
            _voiceVolumeSlider.value = Manager_Audio.Instance.VoiceVolume;
            _sfxVolumeSlider.value = Manager_Audio.Instance.SfxVolume;

            // Add listeners to each slider's onValueChanged event.
            // This will call the appropriate method in the AudioManager when a slider is moved.
            _mainVolumeSlider.onValueChanged.AddListener(Manager_Audio.Instance.SetMainVolume);
            _musicVolumeSlider.onValueChanged.AddListener(Manager_Audio.Instance.SetMusicVolume);
            _voiceVolumeSlider.onValueChanged.AddListener(Manager_Audio.Instance.SetVoiceVolume);
            _sfxVolumeSlider.onValueChanged.AddListener(Manager_Audio.Instance.SetSfxVolume);
        }
    }
}