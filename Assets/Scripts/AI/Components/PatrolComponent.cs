using System;
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
            GoToNextPatrolPoint();
    }

    private void GoToNextPatrolPoint()
    {
        movement.MoveTo(patrolPoints[patrolIndex].position);
        patrolIndex = (patrolIndex + 1) % patrolPoints.Length; 
    }
}
