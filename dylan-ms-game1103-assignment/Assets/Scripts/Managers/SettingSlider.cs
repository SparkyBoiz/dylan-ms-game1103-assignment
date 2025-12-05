using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SettingSlider : MonoBehaviour
{
    [SerializeField] private SettingType _settingType;
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        Debug.Log($"[{gameObject.name}] Awake: Slider component found for {_settingType}.");
    }

    public void Initialize()
    {
        Debug.Log($"[{gameObject.name}] Initialize() called for SettingType: {_settingType}");
        if (Manager_Settings.Instance == null)
        {
            Debug.LogError($"[{gameObject.name}] Initialize failed: Manager_Settings.Instance is null.");
            return;
        }

        _slider.onValueChanged.AddListener(OnSliderValueChanged);

        Manager_Settings.Instance.OnSettingsChanged += OnSettingsChanged;
        Debug.Log($"[{gameObject.name}] Subscribed to Manager_Settings.OnSettingsChanged.");

        UpdateSliderValue();
    }

    private void OnSettingsChanged(SettingType type)
    {
        if (type == _settingType)
            UpdateSliderValue();
    }

    private void OnSliderValueChanged(float value)
    {
        Debug.Log($"[{gameObject.name}] OnSliderValueChanged: User set value to {value}. Notifying Manager_Settings.");
        Manager_Settings.Instance.SetSetting(_settingType, value);
    }

    private void UpdateSliderValue()
    {
        float settingValue = Manager_Settings.Instance.GetSetting(_settingType);
        Debug.Log($"[{gameObject.name}] UpdateSliderValue: Setting slider value to {settingValue}.");
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