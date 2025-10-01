using UnityEngine;

public class HealComponent : MonoBehaviour
{
    [SerializeField] private float visionRange = 10f;
    [SerializeField] private float healRange = 5;
    private bool isHealing;

    [SerializeField] public HealthComponent otherHealth;
    [SerializeField] public Transform otherTransform;
    [SerializeField] private string allyTag;

    [SerializeField] private float healCooldown = 0.5f;
    private float nextHealTime = 0f;

    public bool AllyInVisionRange()
    {
        if (otherTransform == null) return false;
        return Vector3.Distance(transform.position, otherTransform.position) <= visionRange;
    }

    public bool AllyInHealRange()
    {
        if (otherTransform == null) return false;
        return Vector3.Distance(transform.position, otherTransform.position) <= healRange;
    }

    public bool NeedsHeal()
    {
        return otherHealth != null && otherHealth.currentHealth < otherHealth.maxHealth;
    }

    public void HealAlly()
    {
        if (otherHealth != null && Time.time >= nextHealTime)
        {
            if (otherHealth.currentHealth < otherHealth.maxHealth)
            {
                otherHealth.AddHealth(10);
                Debug.Log("Curando al jugador: " + otherHealth.name);
            }

            nextHealTime = Time.time + healCooldown;
        }
    }
}
