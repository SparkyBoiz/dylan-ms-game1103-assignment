using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonManager : MonoBehaviour
{
    public void PlayGame()
    {
        Manager_Transition.Instance.LoadScene("Level_01");
    }

    public void OpenOptions()
    {
        Manager_Transition.Instance.LoadScene("Menu_Options");
    }

    public void Back()
    {
        Manager_Transition.Instance.LoadScene("Menu_Main");
    }
    
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


}
