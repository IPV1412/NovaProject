using System.Collections;
using UnityEngine;

public class KamikazeComponent : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float KamikazeRange = 1f;

    public float timeLeft = .5f;
    private bool hasExploded = false;

    private MovementComponent movement;
    public GameObject explosionPrefab;

    private void Start()
    {
        movement = GetComponent<MovementComponent>();
    }
    public bool PlayerInKamikazeRange()
    {
        Vector3 flatEnemy = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flatPlayer = new Vector3(player.position.x, 0, player.position.z);

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
        Destroy(gameObject, timeLeft);
    }
}
