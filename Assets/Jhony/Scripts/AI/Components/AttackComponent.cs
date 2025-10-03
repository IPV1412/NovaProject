using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1.5f;
    public int Damage = 10; 
    private float lastAttackTime = 0;

    public void Attack(Transform target)
    {
        transform.LookAt(target);


        if (Time.time > lastAttackTime + attackCooldown)
        {
            Debug.Log("Golpe");
            lastAttackTime= Time.time;
        }
    }
}
