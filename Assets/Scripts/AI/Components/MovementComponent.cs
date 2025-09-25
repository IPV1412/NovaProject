using System;
using UnityEngine;
using UnityEngine.AI;

public class MovementComponent : MonoBehaviour
{
    public NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 target)
    {
        agent.SetDestination(target);
    }

    public void Stop()
    {
        agent.SetDestination(transform.position);
    }

    public bool ReachedDestination(float tolerance = 0.5f)
    {
        return !agent.pathPending && agent.remainingDistance <= tolerance;
    }

}
