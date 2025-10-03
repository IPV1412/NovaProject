using UnityEngine;

public class ShieldAI : MonoBehaviour
{
    private VisionComponent vision;
    private MovementComponent movement;
    private PatrolComponent patrol;
    private HealthComponent health;
    private HealComponent heal;

    public float heightOffset = 1f;
    void Start()
    {
        vision = GetComponent<VisionComponent>();
        movement = GetComponent<MovementComponent>();
        patrol = GetComponent<PatrolComponent>();
        health = GetComponent<HealthComponent>();
        heal = GetComponent<HealComponent>();

        movement.agent.baseOffset = heightOffset;
    }

    void Update()
    {
        if (health.ChechIfDead())
        {
            Debug.Log("ESTOY MUERTO");
            Destroy(gameObject);
        }

        if (heal.AllyNeedsHeal())
        {
            movement.MoveTo(heal.targetTransform.position);
            heal.HealAlly();
            Debug.Log("Me dirigo a curar");
        }
        else
        {
            patrol.Patrol();
            Debug.Log("Patrullando");
        }
    }
    
}
