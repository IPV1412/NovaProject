using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    public Health targetHealth;
    public Slider healthSlider;
    public Transform sliderPivot;
    public bool hideWhenFull = true;

    void Start()
    {
        if (targetHealth != null)
        {
            healthSlider.maxValue = targetHealth.MaxHealth;
            healthSlider.value = targetHealth.CurrentHealth;
        }
    }

    void Update()
    {
        if (targetHealth != null)
        {
            healthSlider.value = targetHealth.CurrentHealth;

            if (sliderPivot != null && Camera.main != null)
                sliderPivot.LookAt(Camera.main.transform.position);
            
            if (hideWhenFull)
                sliderPivot.gameObject.SetActive(healthSlider.value < targetHealth.MaxHealth);
        }
    }
}