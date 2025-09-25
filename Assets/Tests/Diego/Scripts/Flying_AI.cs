using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Flying_AI : MonoBehaviour
{
    private VisionComponent vision;
    private MovementComponent movement;
    private PatrolComponent patrol;
    private ShotComponent shot;
    private KamikazeComponent kamikaze;

    public float heightOffset = 3f;

    void Start()
    {
        vision = GetComponent<VisionComponent>();
        movement = GetComponent<MovementComponent>();
        kamikaze = GetComponent<KamikazeComponent>(); 
        patrol = GetComponent<PatrolComponent>();
        shot = GetComponent<ShotComponent>();

        movement.agent.baseOffset = heightOffset;

    }
    void Update()
    {
        if (vision.PlayerInVision())
        {
            movement.MoveTo(vision.Player.position);
            if (kamikaze.PlayerInKamikazeRange())
            {
                Debug.Log("Me mato");
                kamikaze.AutoExplode();
            }
        }
        if (shot.PlayerInShotRange())
        {
            shot.Shot();
            Debug.Log("Disparando");
        }
        
        patrol.Patrol();

    }
}
