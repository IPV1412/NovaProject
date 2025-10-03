using UnityEngine;

public class ShotComponent : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private float shotRange = 10.0f;

    [SerializeField] private GameObject projectilePrefab; 
    [SerializeField] private Transform firePoint;         
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float shotCooldown = 1.5f;  

    private float lastShotTime = 0f;

    public bool PlayerInShotRange()
    {
        return Vector3.Distance(transform.position, player.position) <= shotRange && Vector3.Distance(transform.position, player.position) >= 5;
    }

    public void Shot()
    {
        if (Time.time > lastShotTime + shotCooldown)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.forward * projectileSpeed;
            }

            lastShotTime = Time.time;
            Debug.Log("Disparo enemigo");
        }
    }
}
