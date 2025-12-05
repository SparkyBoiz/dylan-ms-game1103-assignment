using UnityEngine;
using UnityEngine.UI;

public class Options_UI : MonoBehaviour
{
    private void Start()
    {
        Slider_Setting[] sliders = GetComponentsInChildren<Slider_Setting>(true);
        Debug.Log($"[Options_UI] Start: Found {sliders.Length} Slider_Setting components.");
        foreach (var slider in sliders)
        {
            slider.Initialize();
        }
    }
}