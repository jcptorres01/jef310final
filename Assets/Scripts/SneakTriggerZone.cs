using UnityEngine;

public class SneakTriggerZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovementBehavior player = other.GetComponentInParent<PlayerMovementBehavior>();

        if (player != null)
        {
            if (player.isSneaking == true)
            {
                Debug.Log("Player entered while sneaking.");
                // Do sneaking-specific logic here
            }
            else
            {
                Debug.Log("Player entered but is NOT sneaking.");
                // Optional: handle non-sneak case
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerMovementBehavior player = other.GetComponentInParent<PlayerMovementBehavior>();

        if (player != null)
        {
            if (player.isSneaking == true)
            {
                Debug.Log("Player is currently sneaking inside the zone.");
                // Useful if you want continuous checking
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovementBehavior player = other.GetComponentInParent<PlayerMovementBehavior>();

        if (player != null)
        {
            Debug.Log("Player exited the trigger zone.");
            // Cleanup / reset logic here
        }
    }
}