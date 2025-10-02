using UnityEngine;
using UnityEngine.AI;

public class EscapeComponent : MonoBehaviour
{
    private HealthComponent enemyHealth;
    public Transform escapePoint;
    private void Start()
    {
        enemyHealth = GetComponent<HealthComponent>();
    }
    public bool EnemyLowLife()
    {
        return enemyHealth.currentHealth <= 30;
    }
}
