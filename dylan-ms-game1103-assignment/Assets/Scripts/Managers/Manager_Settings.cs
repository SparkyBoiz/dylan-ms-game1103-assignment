using UnityEngine;

public enum SettingType
{
    Main,
    Music,
    Voice,
    Sfx,
    MouseX,
    MouseY,
    ShaderQuality,
    ShadowQuality
}

public class Manager_Settings : Singleton<Manager_Settings>
{
    [Header("Volume Controls")]
    [Range(0f, 1f)] public float MainVolume = 1.0f;
    [Range(0f, 1f)] public float MusicVolume = 1.0f;
    [Range(0f, 1f)] public float VoiceVolume = 1.0f;
    [Range(0f, 1f)] public float SfxVolume = 1.0f;
    [Header("Mouse Sensitivity")]
    [Range(0.1f, 5f)] public float MouseSensitivityX = 1.0f;
    [Range(0.1f, 5f)] public float MouseSensitivityY = 1.0f;
    [Header("Graphics Quality")]
    public int ShaderQuality = 0;
    public int ShadowQuality = 0;

    public System.Action<SettingType> OnSettingsChanged;

    protected override void OnAwake()
    {
        base.OnAwake();
        LoadSettings();
        ApplyGraphicsSettings();
    }

    public void SetSetting(SettingType type, float value)
    {
        switch (type)
        {
            case SettingType.Main: MainVolume = Mathf.Clamp01(value); break;
            case SettingType.Music: MusicVolume = Mathf.Clamp01(value); break;
            case SettingType.Voice: VoiceVolume = Mathf.Clamp01(value); break;
            case SettingType.Sfx: SfxVolume = Mathf.Clamp01(value); break;
            case SettingType.MouseX: MouseSensitivityX = Mathf.Clamp(value, 0.1f, 5f); break;
            case SettingType.MouseY: MouseSensitivityY = Mathf.Clamp(value, 0.1f, 5f); break;
            case SettingType.ShaderQuality:
                ShaderQuality = (int)value;
                break;
            case SettingType.ShadowQuality:
                ShadowQuality = (int)value;
                break;
        }

        SaveSettings();
        OnSettingsChanged?.Invoke(type);
    }

    public float GetSetting(SettingType type)
    {
        switch (type)
        {
            case SettingType.Main: return MainVolume;
            case SettingType.Music: return MusicVolume;
            case SettingType.Voice: return VoiceVolume;
            case SettingType.Sfx: return SfxVolume;
            case SettingType.MouseX: return MouseSensitivityX;
            case SettingType.MouseY: return MouseSensitivityY;
            case SettingType.ShaderQuality: return ShaderQuality;
            case SettingType.ShadowQuality: return ShadowQuality;
            default: return 1.0f;
        }
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("MainVolume", MainVolume);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("VoiceVolume", VoiceVolume);
        PlayerPrefs.SetFloat("SfxVolume", SfxVolume);
        PlayerPrefs.SetFloat("MouseSensitivityX", MouseSensitivityX);
        PlayerPrefs.SetFloat("MouseSensitivityY", MouseSensitivityY);
        PlayerPrefs.SetInt("ShaderQuality", ShaderQuality);
        PlayerPrefs.SetInt("ShadowQuality", ShadowQuality);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        MainVolume = PlayerPrefs.GetFloat("MainVolume", 1.0f);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        VoiceVolume = PlayerPrefs.GetFloat("VoiceVolume", 1.0f);
        SfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1.0f);
        MouseSensitivityX = PlayerPrefs.GetFloat("MouseSensitivityX", 1.0f);
        MouseSensitivityY = PlayerPrefs.GetFloat("MouseSensitivityY", 1.0f);
        ShaderQuality = PlayerPrefs.GetInt("ShaderQuality", QualitySettings.GetQualityLevel());
        ShadowQuality = PlayerPrefs.GetInt("ShadowQuality", (int)QualitySettings.shadowResolution);
    }

    public void ApplyGraphicsSettings()
    {
        QualitySettings.SetQualityLevel(ShaderQuality, true);
        QualitySettings.shadowResolution = (ShadowResolution)ShadowQuality;
        Debug.Log($"Applied Graphics Settings: ShaderQuality={ShaderQuality}, ShadowQuality={ShadowQuality}");
    }
}