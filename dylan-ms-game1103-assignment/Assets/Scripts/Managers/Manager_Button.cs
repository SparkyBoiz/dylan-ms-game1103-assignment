using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip buttonClickSfx;

    public void PlayGame()
    {
        PlayClickSound();
        Manager_Transition.Instance.LoadScene("Level_01");
    }

    public void OpenOptions()
    {
        PlayClickSound();
        Manager_Transition.Instance.LoadScene("Menu_Options");
    }

    public void Back()
    {
        PlayClickSound();
        Manager_Transition.Instance.LoadScene("Menu_Main");
    }
    
    public void QuitGame()
    {
        PlayClickSound();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    private void PlayClickSound()
    {
        if (buttonClickSfx != null && Manager_Audio.Instance != null)
        {
            // Play at camera position for consistent volume regardless of where the button is.
            Manager_Audio.Instance.PlaySoundAtPoint(buttonClickSfx, Camera.main.transform.position);
        }
    }
}
