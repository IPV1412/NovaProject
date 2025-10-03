using UnityEngine;
using UnityEngine.AI;

public class MovementComponent : MonoBehaviour
{
    public NavMeshAgent agent;
    public float targetOffset = 1f;
    public float smoothSpeed = 1f;

    private HealthComponent health;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<HealthComponent>();
    }
    private void Update()
    {
        if (health.ChechIfDead())
        {
            if (agent != null) agent.enabled = false;
        }
    }
    public void MoveTo(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Vector3 stopPosition = target - direction * targetOffset;
        agent.SetDestination(stopPosition);
    }

    public void Stop()
    {
        agent.SetDestination(transform.position);
    }

    public bool ReachedDestination(float tolerance = 0.5f)
    {
        if (agent == null || !agent.isOnNavMesh) return false;
        return !agent.pathPending && agent.remainingDistance <= tolerance;
    }

}
