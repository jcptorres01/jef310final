using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("Vision Settings")]
    public float viewDistance = 15f;
    [Range(0, 360)]
    public float viewAngle = 90f;
    public LayerMask obstacleMask;
    public Transform player;

    public bool CanSeePlayer { get; private set; }

    void Update()
    {
        Vector3 dirToPlayer = (player.position - transform.position);
        float distance = dirToPlayer.magnitude;

        if (distance > viewDistance)
        {
            CanSeePlayer = false;
            return;
        }

        float angle = Vector3.Angle(transform.forward, dirToPlayer.normalized);
        if (angle > viewAngle / 2f)
        {
            CanSeePlayer = false;
            return;
        }

        if (Physics.Raycast(transform.position + Vector3.up, dirToPlayer.normalized, distance, obstacleMask))
        {
            CanSeePlayer = false;
            return;
        }

        CanSeePlayer = true;
    }
}
