using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("Vision Settings")]
    public float viewDistance = 15f;

    [Range(0, 360)]
    public float viewAngle = 90f;

    public LayerMask obstacleMask;
    public Transform player;

    [Header("Attack Settings")]
    public float attackRange = 2f;

    private PlayerMovementBehavior playerMovement;
    private EnemyAttack enemyAttack;

    public bool CanSeePlayer { get; private set; }

    private void Start()
    {
        if (player != null)
        {
            playerMovement =
                player.GetComponent<PlayerMovementBehavior>();
        }

        enemyAttack = GetComponent<EnemyAttack>();
    }

    void Update()
    {
        if (player == null || playerMovement == null)
        {
            CanSeePlayer = false;
            return;
        }

        // PLAYER IS HIDDEN
        if (playerMovement.isHidden)
        {
            CanSeePlayer = false;
            return;
        }

        Vector3 dirToPlayer =
            (player.position - transform.position);

        float distance = dirToPlayer.magnitude;

        // TOO FAR
        if (distance > viewDistance)
        {
            CanSeePlayer = false;
            return;
        }

        // OUTSIDE VIEW ANGLE
        float angle =
            Vector3.Angle(transform.forward,
            dirToPlayer.normalized);

        if (angle > viewAngle / 2f)
        {
            CanSeePlayer = false;
            return;
        }

        // WALL BLOCKING VISION
        if (Physics.Raycast(
            transform.position + Vector3.up,
            dirToPlayer.normalized,
            distance,
            obstacleMask))
        {
            CanSeePlayer = false;
            return;
        }

        // PLAYER IS VISIBLE
        CanSeePlayer = true;

        // ATTACK IF CLOSE ENOUGH
        if (distance <= attackRange)
        {
            enemyAttack.TryAttack();
        }
    }
}