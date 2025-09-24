using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ControlVideo : MonoBehaviour
{
    public Button displayToggleButton;
    public Slider brightnessSlider;
    public Image brightnessOverlay;
    public TextMeshProUGUI brightnessText;

    private float sliderValue;

    private void Start()
    {
        if (displayToggleButton != null)
            displayToggleButton.onClick.AddListener(ToggleDisplayMode);

        sliderValue = PlayerPrefs.GetFloat("Brightness", 0.5f);
        brightnessSlider.value = sliderValue;
        ApplyBrightness(sliderValue);
    }

    public void OnBrightnessChanged(float value)
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("Brightness", sliderValue);
        ApplyBrightness(sliderValue);
    }

    private void ApplyBrightness(float value)
    {
        if (brightnessOverlay != null)
        {
            Color c = brightnessOverlay.color;
            brightnessOverlay.color = new Color(c.r, c.g, c.b, value);
        }

        if (brightnessText != null)
        {
            float percent = (1f - value / 0.9f) * 100f;
            brightnessText.text = Mathf.RoundToInt(percent) + "%";
        }
    }

    private void ToggleDisplayMode()
    {
        if (Screen.fullScreen)
            Screen.fullScreenMode = FullScreenMode.Windowed;
        else
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }
}
