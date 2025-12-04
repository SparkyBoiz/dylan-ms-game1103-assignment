using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [Header("Volume Sliders")]
    [SerializeField] private Slider _mainVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _voiceVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;

    private void OnEnable()
    {
        if (Manager_Audio.Instance != null)
        {
            _mainVolumeSlider.value = Manager_Audio.Instance.MainVolume;
            _musicVolumeSlider.value = Manager_Audio.Instance.MusicVolume;
            _voiceVolumeSlider.value = Manager_Audio.Instance.VoiceVolume;
            _sfxVolumeSlider.value = Manager_Audio.Instance.SfxVolume;

            _mainVolumeSlider.onValueChanged.AddListener(Manager_Audio.Instance.SetMainVolume);
            _musicVolumeSlider.onValueChanged.AddListener(Manager_Audio.Instance.SetMusicVolume);
            _voiceVolumeSlider.onValueChanged.AddListener(Manager_Audio.Instance.SetVoiceVolume);
            _sfxVolumeSlider.onValueChanged.AddListener(Manager_Audio.Instance.SetSfxVolume);
        }
    }

    private void OnDisable()
    {
        if (Manager_Audio.Instance != null)
        {
            _mainVolumeSlider.onValueChanged.RemoveListener(Manager_Audio.Instance.SetMainVolume);
            _musicVolumeSlider.onValueChanged.RemoveListener(Manager_Audio.Instance.SetMusicVolume);
            _voiceVolumeSlider.onValueChanged.RemoveListener(Manager_Audio.Instance.SetVoiceVolume);
            _sfxVolumeSlider.onValueChanged.RemoveListener(Manager_Audio.Instance.SetSfxVolume);
        }
    }
}