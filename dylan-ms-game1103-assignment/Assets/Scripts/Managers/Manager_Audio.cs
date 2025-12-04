using System;
using UnityEngine;

/// <summary>
/// A serializable class to hold a named AudioClip.
/// </summary>
[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class Manager_Audio : MonoBehaviour
{
    public static Manager_Audio Instance { get; private set; }

    [Header("Volume Controls")]
    [Range(0f, 1f)] public float MainVolume = 1.0f;
    [Range(0f, 1f)] public float MusicVolume = 1.0f;
    [Range(0f, 1f)] public float VoiceVolume = 1.0f;
    [Range(0f, 1f)] public float SfxVolume = 1.0f;

    [Header("Audio Sources")]
    [Tooltip("The AudioSource for playing music tracks.")]
    [SerializeField] private AudioSource _musicSource;
    [Tooltip("The AudioSource for playing voice clips.")]
    [SerializeField] private AudioSource _voiceSource;
    [Tooltip("The AudioSource for playing sound effects.")]
    [SerializeField] private AudioSource _sfxSource;

    [Header("Audio Clips")]
    [Tooltip("List of music tracks available to be played.")]
    [SerializeField] private Sound[] _musicTracks;
    [Tooltip("List of voice clips available to be played.")]
    [SerializeField] private Sound[] _voiceClips;
    [Tooltip("List of sound effects available to be played.")]
    [SerializeField] private Sound[] _sfxClips;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of the audio manager exists.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object across scene loads.
        }
        else
        {
            Destroy(gameObject);
        }

        LoadSettings();
    }

    private void Update()
    {
        // Apply volume settings
        if (_musicSource != null) _musicSource.volume = MainVolume * MusicVolume;
        if (_voiceSource != null) _voiceSource.volume = MainVolume * VoiceVolume;
        if (_sfxSource != null) _sfxSource.volume = MainVolume * SfxVolume;
    }

    /// <summary>
    /// Plays a voice clip by name. Stops any currently playing voice clip.
    /// </summary>
    /// <param name="name">The name of the voice clip to play.</param>
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

    /// <summary>
    /// Plays a sound effect by name. Can overlap with other sound effects.
    /// </summary>
    /// <param name="name">The name of the sound effect to play.</param>
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

    /// <summary>
    /// Sets the main volume level.
    /// </summary>
    /// <param name="volume">The new volume level (0.0 to 1.0).</param>
    public void SetMainVolume(float volume)
    {
        MainVolume = volume;
        SaveSettings();
    }

    /// <summary>
    /// Sets the music volume level.
    /// </summary>
    /// <param name="volume">The new volume level (0.0 to 1.0).</param>
    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        SaveSettings();
    }

    /// <summary>
    /// Sets the voice volume level.
    /// </summary>
    /// <param name="volume">The new volume level (0.0 to 1.0).</param>
    public void SetVoiceVolume(float volume)
    {
        VoiceVolume = volume;
        SaveSettings();
    }

    /// <summary>
    /// Sets the SFX volume level.
    /// </summary>
    /// <param name="volume">The new volume level (0.0 to 1.0).</param>
    public void SetSfxVolume(float volume)
    {
        SfxVolume = volume;
        SaveSettings();
    }

    /// <summary>
    /// Saves the current volume settings to PlayerPrefs.
    /// </summary>
    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("MainVolume", MainVolume);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("VoiceVolume", VoiceVolume);
        PlayerPrefs.SetFloat("SfxVolume", SfxVolume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads volume settings from PlayerPrefs.
    /// </summary>
    private void LoadSettings()
    {
        MainVolume = PlayerPrefs.GetFloat("MainVolume", 1.0f);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        VoiceVolume = PlayerPrefs.GetFloat("VoiceVolume", 1.0f);
        SfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1.0f);
    }
}
