using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingBehavior : MonoBehaviour
{
    [Header("References")]
    public Transform gun;
    public Transform spawner;
    public GameObject projectilePrefab;

    [Header("Projectile Settings")]
    public float projectileSpeed = 10f;
    public float secondsPerLaunch = 2f;

    private float secondsElapsed = 0f;

    private EnemyVision enemyVision;

    private void Start()
    {
        enemyVision = GetComponent<EnemyVision>();
    }

    void Update()
    {
        // STOP if enemy cannot see player
        if (enemyVision == null || !enemyVision.CanSeePlayer)
        {
            return;
        }

        secondsElapsed += Time.deltaTime;

        if (secondsElapsed >= secondsPerLaunch)
        {
            GameObject projectile =
                Instantiate(
                    projectilePrefab,
                    spawner.position,
                    spawner.rotation
                );

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = spawner.forward * projectileSpeed;
            }

            secondsElapsed = 0f;
        }
    }
}