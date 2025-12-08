using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Slider_Setting : MonoBehaviour
{
    [SerializeField] private SettingType _settingType;
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void Initialize()
    {
        if (Manager_Settings.Instance == null)
        {
            return;
        }

        _slider.onValueChanged.AddListener(OnSliderValueChanged);

        Manager_Settings.Instance.OnSettingsChanged += OnSettingsChanged;

        UpdateSliderValue();
    }

    private void OnSettingsChanged(SettingType type)
    {
        if (type == _settingType)
            UpdateSliderValue();
    }

    private void OnSliderValueChanged(float value)
    {
        float valueToSet = value;
        if (IsVolumeSlider())
        {
            valueToSet = 1f - value;
        }
        Manager_Settings.Instance.SetSetting(_settingType, valueToSet);
    }

    private void UpdateSliderValue()
    {
        float settingValue = Manager_Settings.Instance.GetSetting(_settingType);
        if (IsVolumeSlider())
        {
            _slider.SetValueWithoutNotify(1f - settingValue);
        }
        else
        {
            _slider.SetValueWithoutNotify(settingValue);
        }
    }

    private void OnDestroy()
    {
        if (Manager_Settings.Instance != null)
        {
            Manager_Settings.Instance.OnSettingsChanged -= OnSettingsChanged;
        }
    }

    private bool IsVolumeSlider()
    {
        return _settingType == SettingType.Main ||
               _settingType == SettingType.Music ||
               _settingType == SettingType.Sfx ||
               _settingType == SettingType.Voice;
    }
}