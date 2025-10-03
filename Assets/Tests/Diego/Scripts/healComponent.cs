using UnityEngine;

public class HealComponent : MonoBehaviour
{
    [HideInInspector] public HealthComponent targetHealth;
    [HideInInspector] public Transform targetTransform;
    private bool inRange = false;
    [SerializeField] private string allyTag;

    [SerializeField] private float healCooldown = 0.5f;
    private float nextHealTime = 0f;

    public bool AllyInVisionRange()
    {
        return inRange; 
    }
   
    public bool AllyNeedsHeal()
    {
        return targetHealth != null && targetHealth.currentShield < targetHealth.maxShield;
    }
    public void HealAlly()
    {
        if (targetHealth != null && Time.time >= nextHealTime)
        {
            if (targetHealth.currentShield < targetHealth.maxShield)
            {
                targetHealth.AddShield(10);
                Debug.Log("Curando al jugador: " + targetHealth.name);
            }

            nextHealTime = Time.time + healCooldown;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(allyTag))
        {
            targetHealth = other.GetComponent<HealthComponent>();
            targetTransform = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(allyTag))
        {
            targetHealth = null;
            targetTransform = null;
        }
    }
}
