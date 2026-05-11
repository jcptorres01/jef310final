using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class LongRangeEnemyNavMeshMovement : MonoBehaviour
{
    public NavMeshAgent agent;

    [Header("Movement")]
    public float moveSpeed = 4f;
    public float rotationSpeed = 8f;

    [Header("Distance Settings")]
    public float preferredDistance = 18f;
    public float retreatDistance = 10f;
    public float retreatAmount = 6f;

    private EnemyVision vision;
    private Transform player;

    private enum EnemyState
    {
        MovingToRange,
        Retreating,
        HoldingPosition
    }

    private EnemyState currentState;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        vision = GetComponent<EnemyVision>();
        player = vision.player;

        agent.speed = moveSpeed;

        preferredDistance =
            Mathf.Min(preferredDistance,
            vision.viewDistance - 1f);

        retreatDistance =
            Mathf.Min(retreatDistance,
            preferredDistance - 1f);

        // Let NavMesh rotate normally
        agent.updateRotation = true;
    }

    void Update()
    {
        if (!vision.CanSeePlayer)
        {
            agent.ResetPath();
            return;
        }

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        Vector3 awayDirection =
            (transform.position - player.position).normalized;

        // --------------------------------
        // STATE SWITCHING
        // --------------------------------

        switch (currentState)
        {
            // -----------------------------
            // RETREATING
            // -----------------------------
            case EnemyState.Retreating:

                // Once enemy escapes retreat range,
                // transition into repositioning
                if (distance >= retreatDistance)
                {
                    currentState =
                        EnemyState.MovingToRange;
                }

                break;

            // -----------------------------
            // MOVING TO PREFERRED RANGE
            // -----------------------------
            case EnemyState.MovingToRange:

                // Once enemy reaches preferred range,
                // begin holding/shooting again
                if (distance >= preferredDistance)
                {
                    currentState =
                        EnemyState.HoldingPosition;
                }

                // Player rushed enemy again
                else if (
                    retreatDistance > 0f &&
                    distance < retreatDistance)
                {
                    currentState =
                        EnemyState.Retreating;
                }

                break;

            // -----------------------------
            // HOLD POSITION
            // -----------------------------
            case EnemyState.HoldingPosition:

                // Player too close again
                if (
                    retreatDistance > 0f &&
                    distance < retreatDistance)
                {
                    currentState =
                        EnemyState.Retreating;
                }

                // Player too far away
                else if (distance > preferredDistance)
                {
                    currentState =
                        EnemyState.MovingToRange;
                }

                break;
        }

        // --------------------------------
        // STATE BEHAVIOR
        // --------------------------------

        switch (currentState)
        {
            // --------------------------------
            // RETREAT
            // --------------------------------
            case EnemyState.Retreating:

                agent.updateRotation = false;

                Vector3 retreatPos =
                    transform.position +
                    awayDirection * retreatAmount;

                if (NavMesh.SamplePosition(
                    retreatPos,
                    out NavMeshHit retreatHit,
                    3f,
                    NavMesh.AllAreas))
                {
                    agent.SetDestination(
                        retreatHit.position
                    );
                }

                // Face player while backing away
                FacePlayer();

                break;

            // --------------------------------
            // MOVE INTO PREFERRED RANGE
            // --------------------------------
            case EnemyState.MovingToRange:

                // Turn normally
                agent.updateRotation = true;

                Vector3 desiredPos =
                    player.position +
                    awayDirection *
                    preferredDistance;

                if (NavMesh.SamplePosition(
                    desiredPos,
                    out NavMeshHit moveHit,
                    3f,
                    NavMesh.AllAreas))
                {
                    agent.SetDestination(
                        moveHit.position
                    );
                }

                break;

            // --------------------------------
            // HOLD POSITION
            // --------------------------------
            case EnemyState.HoldingPosition:

                agent.ResetPath();

                agent.updateRotation = false;

                FacePlayer();

                break;
        }
    }

    void FacePlayer()
    {
        Vector3 dir =
            player.position - transform.position;

        dir.y = 0f;

        if (dir != Vector3.zero)
        {
            Quaternion target =
                Quaternion.LookRotation(dir);

            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    target,
                    rotationSpeed *
                    Time.deltaTime
                );
        }
    }
}