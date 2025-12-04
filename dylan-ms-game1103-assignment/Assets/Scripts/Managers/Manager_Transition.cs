using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager_Transition : MonoBehaviour
{
    public static Manager_Transition Instance { get; private set; }

    [Tooltip("The UI Image to use for the fade effect.")]
    [SerializeField] private Image fadeImage;

    [Tooltip("The duration of the fade in/out effect in seconds.")]
    [SerializeField] private float fadeDuration = 1.0f;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of the transition manager exists.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object across scene loads.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Public method to start the scene transition.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeTransition(sceneName));
    }

    /// <summary>
    /// Coroutine that handles the fade-out, scene load, and fade-in sequence.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    private IEnumerator FadeTransition(string sceneName)
    {
        yield return Fade(1f); // Fade to black

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return Fade(0f); // Fade in from black
    }

    /// <summary>
    /// Coroutine to fade the screen to a target alpha.
    /// </summary>
    /// <param name="targetAlpha">The target alpha value (0 for transparent, 1 for opaque).</param>
    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeImage == null) yield break;

        float startAlpha = fadeImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / (fadeDuration / 2));
            fadeImage.color = new Color(0, 0, 0, newAlpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }
}
