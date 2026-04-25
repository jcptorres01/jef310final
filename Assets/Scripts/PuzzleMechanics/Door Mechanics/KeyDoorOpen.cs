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
        if (isOpen) return;

        //Key check
        if (playerInventory == null || !playerInventory.HasItem(requiredKey))
        {
            Debug.Log("You need the correct key.");
            return;
        }

        UnlockDoor();
    }

    void UnlockDoor()
    {
        isOpen = true;

        Debug.Log("Door unlocked with key!");
        sceneLoader.Interacting();
        
    }
}