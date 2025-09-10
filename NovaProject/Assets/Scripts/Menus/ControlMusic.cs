using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class ControlMusic : MonoBehaviour
{
    [Header("UI - Siliders")]
    
    public Slider volGeneral, volMusic, volSFX;
    public TextMeshProUGUI textGeneral, textMusic, textSFX;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    private float ConvertToPercent(float value)
    {
        return Mathf.RoundToInt((value + 80) / 80 * 100);
    }

    private void UpdateSliderText(Slider slider, TextMeshProUGUI text)
    {
        if (text != null)
            text.text = ConvertToPercent(slider.value) + "%";
    }

    public void ControlGeneralVol()
    {
        float general = volGeneral.value;
        audioMixer.SetFloat("General", general);
        PlayerPrefs.SetFloat("VolGeneral", general);
        UpdateSliderText(volGeneral, textGeneral);
    }

    public void ControlMusicVol()
    {
        float vmusic = volMusic.value;
        audioMixer.SetFloat("V_Music", vmusic);
        PlayerPrefs.SetFloat("VolMusic", vmusic);
        UpdateSliderText(volMusic, textMusic);
    }

    public void ControlSFXVol()
    {
        float vsfx = volSFX.value;
        audioMixer.SetFloat("V_SFX", vsfx);
        PlayerPrefs.SetFloat("VolSFX", vsfx);
        UpdateSliderText(volSFX, textSFX);
    }

    private void Start()
    {
        volGeneral.value = PlayerPrefs.GetFloat("VolGeneral", -40f);

        ControlGeneralVol();
        ControlMusicVol();
        ControlSFXVol();
    }
}
