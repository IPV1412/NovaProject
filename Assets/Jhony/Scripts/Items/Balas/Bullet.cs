using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Bullet : MonoBehaviour
{
    public enum IgnoreTargetType { None, Player, Enemy }

    [Header("Bullet Settings")]
    public float speed = 20f;
    public float damage = 50f;
    public float maxRange = 10f;

    [Header("Ignore Settings")]
    public IgnoreTargetType ignoreTarget = IgnoreTargetType.None;

    [HideInInspector] public GameObject owner;

    private Vector3 _spawnPosition;
    private Vector3 _direction;
    private Collider _bulletCollider;

    void Start()
    {
        _spawnPosition = transform.position;
        _direction = transform.forward;
        _bulletCollider = GetComponent<Collider>();
    }

    void Update()
    {
        transform.position += _direction * (speed * Time.deltaTime);

        if (Vector3.Distance(_spawnPosition, transform.position) > maxRange)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == owner)
            return;
        
        if ((ignoreTarget == IgnoreTargetType.Player && collision.gameObject.CompareTag("Player")) ||
            (ignoreTarget == IgnoreTargetType.Enemy && collision.gameObject.CompareTag("Enemy")))
        {
            Physics.IgnoreCollision(_bulletCollider, collision.collider); 
            return;
        }
        
        var health = collision.collider.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage, owner != null ? owner : gameObject);
        }

        Destroy(gameObject);
    }
}