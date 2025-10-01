using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    private int healthDamage = 10;

    public int maxShield = 100;
    public int currentShield;
    private int shieldDamage = 5;

    public Slider healthBar;
    public Slider ShieldBar;

    void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;
    }

    void Update()
    {
        healthBar.value = currentHealth;
        ShieldBar.value = currentShield;
        if (Input.GetMouseButtonDown(0))
        {
            if (currentShield > 0)
            {
                SubtractShield(shieldDamage);
            }
            else
            {
                TakeDamage(healthDamage);
            }
        }
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Vida sube a : " + currentHealth);
    }

    public void SubtractShield(int amount)
    {
        currentShield -= amount;
        currentShield = Mathf.Clamp(currentShield, 0, maxShield);

        Debug.Log("Escudo sube a : " + currentShield);

    }
    public void AddHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Vida sube a : " + currentShield);
    }
}