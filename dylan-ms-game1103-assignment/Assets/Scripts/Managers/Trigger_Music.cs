using UnityEngine;

public class Trigger_Music : MonoBehaviour
{
    [Header("Music Settings")]
    [SerializeField] private string musicTrackName;

    void Start()
    {
        // Ensure the audio manager and track name are valid
        if (Manager_Audio.Instance != null && !string.IsNullOrEmpty(musicTrackName))
        {
            Manager_Audio.Instance.PlayMusic(musicTrackName);
        }
    }
}
