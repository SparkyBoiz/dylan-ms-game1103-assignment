using System;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class Manager_Audio : Singleton<Manager_Audio>
{
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

        if (Manager_Settings.Instance != null)
        {
            Manager_Settings.Instance.OnSettingsChanged += OnSettingsChanged;
        }

        ApplyAllVolumeSettings();

        if (_musicTracks.Length > 0)
        {
            PlayMusic(_musicTracks[0].name);
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(_musicTracks, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        _musicSource.clip = s.clip;
        _musicSource.Play();
    }

    public void PlayVoice(string name)
    {
        Sound s = Array.Find(_voiceClips, sound => sound.name == name);
        if (s == null)
        {
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
            return;
        }
        _sfxSource.PlayOneShot(s.clip);
    }

    public void PlaySoundAtPoint(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;

        GameObject tempAudio = new GameObject("TempAudio");
        tempAudio.transform.position = position;
        AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = Manager_Settings.Instance.MainVolume * 
                             Manager_Settings.Instance.SfxVolume;
        audioSource.Play();
        Destroy(tempAudio, clip.length);
    }

    protected override void OnDestroy()
    {
        if (Manager_Settings.Instance != null)
        {
            Manager_Settings.Instance.OnSettingsChanged -= OnSettingsChanged;
        }
        base.OnDestroy();
    }

    private void OnSettingsChanged(SettingType type)
    {
        switch (type)
        {
            case SettingType.Main:
            case SettingType.Music:
            case SettingType.Voice:
            case SettingType.Sfx:
                ApplyAllVolumeSettings();
                break;
        }
    }

    private void ApplyAllVolumeSettings()
    {
        if (Manager_Settings.Instance == null) return;

        float mainVolume = Manager_Settings.Instance.MainVolume;

        if (_musicSource != null) _musicSource.volume = mainVolume * Manager_Settings.Instance.MusicVolume;
        if (_voiceSource != null) _voiceSource.volume = mainVolume * Manager_Settings.Instance.VoiceVolume;
        if (_sfxSource != null) _sfxSource.volume = mainVolume * Manager_Settings.Instance.SfxVolume;
    }
}
