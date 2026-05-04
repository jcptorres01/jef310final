using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] // CHANGED
public class EnemyNavMeshMovement : MonoBehaviour
{
    public NavMeshAgent agent;

    [Header("Movement")]
    public float chaseSpeed = 4f;
    public float rotationSpeed = 8f;

    [Header("Animation")]
    //public Animator animator;
    //public string speedParam = "Speed";
    //public string chasingParam = "IsChasing";

    private EnemyVision vision;
    private Transform player;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>(); // NEW
        vision = GetComponent<EnemyVision>();
        player = vision.player;

        agent.speed = chaseSpeed; // NEW
    }

    void Update()
    {
        if (!vision.CanSeePlayer)
        {
            //animator.SetBool(chasingParam, false);
            //animator.SetFloat(speedParam, 0f);

            agent.ResetPath(); // NEW
            return;
        }

        //animator.SetBool(chasingParam, true);

        agent.SetDestination(player.position); // NEW

        //animator.SetFloat(speedParam, agent.velocity.magnitude); // NEW
    }
}