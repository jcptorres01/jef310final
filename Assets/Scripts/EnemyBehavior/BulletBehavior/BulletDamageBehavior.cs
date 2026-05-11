using System.Collections.Generic;
using UnityEngine;

public class BulletDamageBehavior : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damageAmount = 10;

    // TRACK WHO WAS ALREADY HIT
    private HashSet<PlayerHealth> hitPlayers =
        new HashSet<PlayerHealth>();

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth =
            GetComponentInParent<PlayerHealth>();

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

        Destroy(this.gameObject);
    }
}
