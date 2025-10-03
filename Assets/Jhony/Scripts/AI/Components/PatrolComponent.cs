using UnityEngine;

public class PatrolComponent : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    private int patrolIndex = 0;

    private MovementComponent movement;

    private void Awake()
    {
        movement = GetComponent<MovementComponent>();
    }
    
    public void StartPatrol()
    {
        if (patrolPoints.Length > 0)
            GoToNextPatrolPoint();
    }

    public void Patrol()
    {
        if (movement.ReachedDestination())
        {
            GoToNextPatrolPoint();
            Debug.Log("Llegué al siguiente punto");
        }
    }

    public void RandomPatrol()
    {
        if (movement.ReachedDestination())
        {
            GoToRandomPatrolPoint();
            Debug.Log("Llegué al siguiente punto random");
        }
    }

    private void GoToNextPatrolPoint()
    {
        movement.MoveTo(patrolPoints[patrolIndex].position);
        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
    }

    private void GoToRandomPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        int randomIndex = Random.Range(0, patrolPoints.Length);

        while (randomIndex == patrolIndex && patrolPoints.Length > 1)
        {
            randomIndex = Random.Range(0, patrolPoints.Length);
        }

        patrolIndex = randomIndex;
        movement.MoveTo(patrolPoints[patrolIndex].position);
    }
}
