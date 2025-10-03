using System.Collections;
using UnityEngine;

public class KamikazeComponent : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float KamikazeRange = 1f;
    private Health health;

    public float timeLeft = .5f;
    private bool hasExploded = false;
    private int bombDamage = 30;

    private MovementComponent movement;
    public GameObject explosionPrefab;

    private void Start()
    {
        movement = GetComponent<MovementComponent>();
        if (player != null)
        {
            health = player.GetComponent<Health>();
            if (health == null)
            {
                Debug.LogWarning("El Player no tiene componente Health asignado.");
            }
        }
    }
    public bool PlayerInKamikazeRange()
    {
        Vector3 flatEnemy = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flatPlayer = new Vector3(player.transform.position.x, 0, player.transform.position.z);

        return Vector3.Distance(flatEnemy, flatPlayer) <= KamikazeRange;
    }
    public void AutoExplode()
    {
        if (hasExploded == false && movement.ReachedDestination())
        {
            hasExploded = true;
            
            if(explosionPrefab != null)
            {
                StartCoroutine(ExplodeAfterDelay());
            }
        }
    }

    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(timeLeft);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        health.SubtractHealth(bombDamage);
        Destroy(gameObject, timeLeft);
    }
}
