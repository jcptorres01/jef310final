using UnityEngine;

public class SneakTriggerZone : MonoBehaviour
{
    public GameObject sneakOverlayUI;

    private PlayerMovementBehavior player;

    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponentInParent<PlayerMovementBehavior>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (player == null) return;

        if (player.isSneaking)
        {
            sneakOverlayUI.SetActive(true);
            player.SetHidden(true);   //PLAYER IS HIDDEN
        }
        else
        {
            sneakOverlayUI.SetActive(false);
            player.SetHidden(false); // NOT HIDDEN
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovementBehavior exitingPlayer = other.GetComponentInParent<PlayerMovementBehavior>();

        if (exitingPlayer != null)
        {
            sneakOverlayUI.SetActive(false);
            exitingPlayer.SetHidden(false); // leaving grass = visible
            player = null;
        }
    }
}