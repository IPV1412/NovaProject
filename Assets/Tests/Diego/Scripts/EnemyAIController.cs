using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    private VisionComponent vision;
    private MovementComponent movement;
    private AttackComponent attack;
    private PatrolComponent patrol;
    private ShotComponent shot;
    private EscapeComponent escape;
    private HealthComponent health;

    private void Awake()
    {
        vision = GetComponent<VisionComponent>();
        movement = GetComponent<MovementComponent>();
        attack = GetComponent<AttackComponent>();
        patrol = GetComponent<PatrolComponent>();
        shot = GetComponent<ShotComponent>();
        escape = GetComponent<EscapeComponent>();
        health = GetComponent<HealthComponent>();
    }

    void Start()
    {
        patrol.StartPatrol();
    }

    private void Update()
    {
        if (health.ChechIfDead())
        {
            //Debug.Log("ESTOY MUERTO");
            Destroy(gameObject);
        }


        if (vision.PlayerInAttackRange())
        {
            //Debug.Log("Estado de ataque");
            attack.Attack(vision.Player);
        }
        else if (vision.PlayerInVision())
        {
            //Debug.Log("Estado de perseguir");
            movement.MoveTo(vision.Player.position);
        }
        else
        {
            //Debug.Log("Estado de patrullaje");
            patrol.Patrol();
        }

        
    }
}
