using System;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

[Prefab("Manager_Audio")]
public class Manager_Audio : Singleton<Manager_Audio>
{
    [Header("Volume Controls")]
    [Range(0f, 1f)] public float MainVolume = 1.0f;
    [Range(0f, 1f)] public float MusicVolume = 1.0f;
    [Range(0f, 1f)] public float VoiceVolume = 1.0f;
    [Range(0f, 1f)] public float SfxVolume = 1.0f;

    [Header("Audio Sources")]
    private AudioSource _musicSource;
    private AudioSource _voiceSource;
    private AudioSource _sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private Sound[] _musicTracks;
    [SerializeField] private Sound[] _voiceClips;
    [SerializeField] private Sound[] _sfxClips;

    protected override void OnAwake()
    {
        base.OnAwake();
        _musicSource = new GameObject("MusicSource").AddComponent<AudioSource>();
        _musicSource.transform.SetParent(transform);
        _musicSource.loop = true;

        _voiceSource = new GameObject("VoiceSource").AddComponent<AudioSource>();
        _voiceSource.transform.SetParent(transform);

        _sfxSource = new GameObject("SfxSource").AddComponent<AudioSource>();
        _sfxSource.transform.SetParent(transform);

        LoadSettings();
        ApplyAllVolumeSettings();
    }

    public void PlayVoice(string name)
    {
        Sound s = Array.Find(_voiceClips, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Voice clip not found: " + name);
            return;
        }
        _voiceSource.Stop();
        _voiceSource.clip = s.clip;
        _voiceSource.Play();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(_sfxClips, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("SFX not found: " + name);
            return;
        }
        _sfxSource.PlayOneShot(s.clip);
    }

    public void SetMainVolume(float volume)
    {
        MainVolume = volume;
        ApplyAllVolumeSettings();
        SaveSettings();
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        ApplyAllVolumeSettings();
        SaveSettings();
    }

    public void SetVoiceVolume(float volume)
    {
        VoiceVolume = volume;
        ApplyAllVolumeSettings();
        SaveSettings();
    }

    public void SetSfxVolume(float volume)
    {
        SfxVolume = volume;
        ApplyAllVolumeSettings();
        SaveSettings();
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("MainVolume", MainVolume);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("VoiceVolume", VoiceVolume);
        PlayerPrefs.SetFloat("SfxVolume", SfxVolume);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        MainVolume = PlayerPrefs.GetFloat("MainVolume", 1.0f);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        VoiceVolume = PlayerPrefs.GetFloat("VoiceVolume", 1.0f);
        SfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1.0f);
    }

    private void ApplyAllVolumeSettings()
    {
        if (_musicSource != null) _musicSource.volume = MainVolume * MusicVolume;
        if (_voiceSource != null) _voiceSource.volume = MainVolume * VoiceVolume;
        if (_sfxSource != null) _sfxSource.volume = MainVolume * SfxVolume;
    }
}
