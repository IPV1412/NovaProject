using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public Transform turretPivot;
    public Transform firePoint;
    public ParticleSystem detectParticles;
    public ParticleSystem shootParticles;
    public float rotationSpeed = 5f;
    public float fireRate = 1f;
    public float detectionRange = 15f;
    public GameObject bulletPrefab;

    Transform player;
    Vector3 initialRotation;
    float nextFireTime;
    bool playerDetected;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        initialRotation = turretPivot.eulerAngles;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            if (!playerDetected)
            {
                detectParticles.Play();
                playerDetected = true;
            }

            Vector3 direction = (player.position - turretPivot.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            turretPivot.rotation = Quaternion.Slerp(turretPivot.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
        else
        {
            if (playerDetected)
            {
                playerDetected = false;
                detectParticles.Stop();
            }

            turretPivot.rotation = Quaternion.Slerp(turretPivot.rotation, Quaternion.Euler(initialRotation), rotationSpeed * Time.deltaTime);
        }
    }

    void Shoot()
    {
        shootParticles.Play();
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward;
        }
    }
}
