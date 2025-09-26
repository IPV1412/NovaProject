using UnityEngine;

public class TurretDeathHandler : MonoBehaviour
{
    private Health _health;
    [SerializeField] public GameObject barPrefab;
    private GameObject barInstance;

    void Start()
    {
        _health = GetComponent<Health>();

        if (_health != null)
            _health.OnDie += HandleDeath;
    }

    void HandleDeath()
    {
        SpawnUpgrade();
        Destroy(gameObject);
    }
    
    void SpawnUpgrade()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.2f , transform.position.z);
        barInstance = Instantiate(barPrefab, spawnPosition, transform.rotation);
    }
}