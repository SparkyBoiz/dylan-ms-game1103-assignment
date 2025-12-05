using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Manager_Transition : Singleton<Manager_Transition>
{
    private Image _fadeImage;

    [SerializeField] private float fadeDuration = 1.0f;

    protected override void OnAwake()
    {
        base.OnAwake();
        CreateFadeUI();
    }

    private void CreateFadeUI()
    {
        if (FindFirstObjectByType<EventSystem>() == null)
        {
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        if (transform.Find("TransitionCanvas")) return;

        GameObject canvasObject = new GameObject("TransitionCanvas");
        canvasObject.transform.SetParent(transform);
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        GameObject imageObject = new GameObject("FadeImage");
        imageObject.transform.SetParent(canvas.transform);
        _fadeImage = imageObject.AddComponent<Image>();
        _fadeImage.rectTransform.anchorMin = Vector2.zero;
        _fadeImage.rectTransform.anchorMax = Vector2.one;
        _fadeImage.rectTransform.offsetMin = Vector2.zero;
        _fadeImage.rectTransform.offsetMax = Vector2.zero;
        _fadeImage.color = new Color(0, 0, 0, 0);
        _fadeImage.raycastTarget = false;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeTransition(sceneName));
    }

    private IEnumerator FadeTransition(string sceneName)
    {
        yield return Fade(1f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return Fade(0f);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (_fadeImage == null)
        {
            yield break;
        }

        _fadeImage.raycastTarget = true;
        float startAlpha = _fadeImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            _fadeImage.color = new Color(0, 0, 0, newAlpha);
            yield return null;
        }

        _fadeImage.color = new Color(0, 0, 0, targetAlpha);
        _fadeImage.raycastTarget = (targetAlpha > 0);
    }
}