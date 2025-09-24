using UnityEngine;

public class VisionComponent : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float visionRange = 10f;
    [SerializeField] private float attackRange = 2;
    
    public Transform Player => player;


    public bool PlayerInVision()
    {
        Vector3 flatEnemy = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flatPlayer = new Vector3(player.position.x, 0, player.position.z);
        
        return Vector3.Distance(flatEnemy, flatPlayer) <= visionRange;
    }
    
    public bool PlayerInAttackRange()
    {
        Vector3 flatEnemy = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 flatPlayer = new Vector3(player.position.x, 0, player.position.z);
        
        return Vector3.Distance(flatEnemy, flatPlayer) <= attackRange;
    }
}
