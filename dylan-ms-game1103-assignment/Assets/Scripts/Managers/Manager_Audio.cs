using System;
using System.Collections;
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
    private AudioSource _musicSource1;
    private AudioSource _musicSource2;
    private AudioSource _activeMusicSource;
    private AudioSource _voiceSource;
    private AudioSource _sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private Sound[] _musicTracks;
    [SerializeField] private Sound[] _voiceClips;
    [SerializeField] private Sound[] _sfxClips;

    private Coroutine _musicFadeCoroutine;

    protected override void OnAwake()
    {
        base.OnAwake();
        DontDestroyOnLoad(gameObject);
        _musicSource1 = new GameObject("MusicSource1").AddComponent<AudioSource>();
        _musicSource1.transform.SetParent(transform);
        _musicSource1.loop = true;

        _musicSource2 = new GameObject("MusicSource2").AddComponent<AudioSource>();
        _musicSource2.transform.SetParent(transform);
        _musicSource2.loop = true;

        _activeMusicSource = _musicSource1;

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
            // Play initial track without fading
            _activeMusicSource.clip = _musicTracks[0].clip;
            _activeMusicSource.Play();
        }
    }

    public void PlayMusic(string name, float fadeDuration = 1.0f)
    {
        Sound s = Array.Find(_musicTracks, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        
        if (_activeMusicSource.clip == s.clip && _activeMusicSource.isPlaying)
        {
            return;
        }

        if (_musicFadeCoroutine != null)
        {
            StopCoroutine(_musicFadeCoroutine);
        }
        _musicFadeCoroutine = StartCoroutine(CrossfadeMusic(s.clip, fadeDuration));
    }

    private IEnumerator CrossfadeMusic(AudioClip newClip, float duration)
    {
        AudioSource oldSource = _activeMusicSource;
        AudioSource newSource = (_activeMusicSource == _musicSource1) ? _musicSource2 : _musicSource1;

        newSource.clip = newClip;
        newSource.Play();

        float timer = 0f;
        float oldSourceStartVolume = oldSource.volume;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            float targetVolume = Manager_Settings.Instance.MainVolume * Manager_Settings.Instance.MusicVolume;
            oldSource.volume = Mathf.Lerp(oldSourceStartVolume, 0, progress);
            newSource.volume = Mathf.Lerp(0, targetVolume, progress);
            yield return null;
        }

        oldSource.Stop();
        _activeMusicSource = newSource;
        _activeMusicSource.volume = Manager_Settings.Instance.MainVolume * Manager_Settings.Instance.MusicVolume;
        _musicFadeCoroutine = null;
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

        // If a fade is not in progress, apply volume directly. Otherwise, the coroutine handles it.
        if (_musicFadeCoroutine == null)
        {
            float musicVolume = mainVolume * Manager_Settings.Instance.MusicVolume;
            if (_activeMusicSource != null) _activeMusicSource.volume = musicVolume;
        }

        if (_voiceSource != null) _voiceSource.volume = mainVolume * Manager_Settings.Instance.VoiceVolume;
        if (_sfxSource != null) _sfxSource.volume = mainVolume * Manager_Settings.Instance.SfxVolume;
    }
}
