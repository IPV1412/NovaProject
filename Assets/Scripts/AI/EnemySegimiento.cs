using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private VisionComponent vision;
    private MovementComponent movement;
    private AttackComponent attack;
    private PatrolComponent patrol;

    private void Awake()
    {
        vision = GetComponent<VisionComponent>();
        movement = GetComponent<MovementComponent>();
        attack = GetComponent<AttackComponent>();
        patrol = GetComponent<PatrolComponent>();
    }

    void Start()
    {
        patrol.StartPatrol();
    }

    void Update()
    {
        if (vision.PlayerInAttackRange())
        {
            Debug.Log("State:Atacando");
            movement.Stop();
            attack.Attack(vision.Player);
            
        }
        else if (vision.PlayerInVision())
        {
            Debug.Log("State:Siguiendo");
            movement.MoveTo(vision.Player.position);
            
        }
        else
        {
            Debug.Log("State:Patrullando");
            patrol.Patrol();
            
        }
    }
}
