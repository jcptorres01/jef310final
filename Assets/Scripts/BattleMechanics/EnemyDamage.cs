using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damageAmount = 10;

    // TRACK WHO WAS ALREADY HIT
    private HashSet<PlayerHealth> hitPlayers =
        new HashSet<PlayerHealth>();

    private void OnEnable()
    {
        // RESET EACH NEW ATTACK
        hitPlayers.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth =
            other.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            // ALREADY HIT THIS PLAYER THIS SWING
            if (hitPlayers.Contains(playerHealth))
                return;

            // DAMAGE PLAYER
            playerHealth.TakeDamage(
                damageAmount,
                transform.position
            );

            // REMEMBER PLAYER
            hitPlayers.Add(playerHealth);
        }
    }
}