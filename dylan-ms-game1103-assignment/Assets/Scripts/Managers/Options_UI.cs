using UnityEngine;
using UnityEngine.UI;

public class Options_UI : MonoBehaviour
{
    private void Start()
    {
        Slider_Setting[] sliders = GetComponentsInChildren<Slider_Setting>(true);
        foreach (var slider in sliders)
        {
            slider.Initialize();
        }
    }
}