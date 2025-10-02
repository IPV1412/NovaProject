using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{

    [HideInInspector] public int maxHealth = 100;
    [HideInInspector] public int currentHealth;
    private int healthDamage = 10;

    [HideInInspector] public int maxShield = 100;
    [HideInInspector] public int currentShield;
    private int shieldDamage = 5;

    public Slider healthBar;
    public Slider ShieldBar;

    [Header("Daño por colisión")]
    [SerializeField] private string damageTag; 
    [SerializeField] private int collisionDamage = 10;

    void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;
    }

    void Update()
    {
        healthBar.value = currentHealth;
        ShieldBar.value = currentShield;
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Vida baja a : " + currentHealth);
    }

    public void SubtractShield(int amount)
    {
        currentShield -= amount;
        currentShield = Mathf.Clamp(currentShield, 0, maxShield);

        Debug.Log("Escudo baja a : " + currentShield);
    }
    public void AddHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Vida sube a : " + currentShield);
    }

    public void AddShield(int amount)
    {
        currentShield += amount;
        currentShield = Mathf.Clamp(currentShield, 0, maxShield);

        Debug.Log("Escudo baja a : " + currentShield);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(damageTag))
        {
            Debug.Log("Colisión con: " + other.name);

            if (currentShield > 0)
                SubtractShield(collisionDamage);
            else
                TakeDamage(collisionDamage);
        }
    }
    public bool ChechIfDead()
    {
        if(currentHealth == 0)
        {
            return true;
        }
        return false;
    }
    public void Dead()
    {
        Destroy(this);
    }
}