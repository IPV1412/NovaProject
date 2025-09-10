using UnityEngine;

public class TurretDeathHandler : MonoBehaviour
{
    private Health _health;

    void Start()
    {
        _health = GetComponent<Health>();

        if (_health != null)
            _health.OnDie += HandleDeath;
    }

    void HandleDeath()
    {
        Destroy(gameObject);
    }
}