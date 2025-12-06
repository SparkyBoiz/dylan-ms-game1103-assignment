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
        Manager_Settings.Instance.SetSetting(_settingType, value);
    }

    private void UpdateSliderValue()
    {
        float settingValue = Manager_Settings.Instance.GetSetting(_settingType);
        _slider.SetValueWithoutNotify(settingValue);
    }

    private void OnDestroy()
    {
        if (Manager_Settings.Instance != null)
        {
            Manager_Settings.Instance.OnSettingsChanged -= OnSettingsChanged;
        }
    }
}