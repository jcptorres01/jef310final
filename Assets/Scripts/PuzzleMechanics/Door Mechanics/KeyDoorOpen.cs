using UnityEngine;

public class KeyDoorOpen : MonoBehaviour, IInteract
{
    [Header("Key Requirement")]
    public WeaponSO requiredKey;
    public PlayerInventory playerInventory;

    [Header("Door Behavior")]
    public bool loadNextSceneInstead = true;
    public LoadNextScene sceneLoader;

    private bool isOpen = false;

    public void Interacting()
    {
        // Prevent multiple activations
        if (isOpen)
            return;

        // Make sure inventory reference exists
        if (playerInventory == null)
        {
            Debug.LogError("PlayerInventory reference is missing!");
            return;
        }

        // Check if player has the required key
        if (!playerInventory.HasItem(requiredKey))
        {
            Debug.Log("You need the correct key.");
            return;
        }

        // Unlock the door
        UnlockDoor();
    }

    void UnlockDoor()
    {
        isOpen = true;

        // Remove the key from inventory (one-time use)
        playerInventory.RemoveItem(requiredKey);

        Debug.Log("Door unlocked with key!");

        // Load next scene
        if (loadNextSceneInstead && sceneLoader != null)
        {
            sceneLoader.Interacting();
        }
    }
}