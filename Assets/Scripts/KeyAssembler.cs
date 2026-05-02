using UnityEngine;

public class KeyAssembler : MonoBehaviour, IInteract
{
    [SerializeField] private WeaponSO keyFragment;
    [SerializeField] private WeaponSO fullKey;

    [SerializeField] private int requiredFragments = 3;

    public void Interacting()
    {
        PlayerInventory player = FindObjectOfType<PlayerInventory>();

        int fragmentCount = CountFragments(player);

        if (fragmentCount >= requiredFragments)
        {
            CraftKey(player);
            Debug.Log("Key crafted!");
        }
        else
        {
            Debug.Log("Not enough fragments.");
        }
    }

    int CountFragments(PlayerInventory player)
    {
        int count = 0;

        foreach (var item in player.inventoryList)
        {
            if (item == keyFragment)
                count++;
        }

        return count;
    }

    void CraftKey(PlayerInventory player)
    {
        int removed = 0;

        for (int i = player.inventoryList.Count - 1; i >= 0; i--)
        {
            if (player.inventoryList[i] == keyFragment)
            {
                player.RemoveItem(player.inventoryList[i]);
                removed++;

                if (removed >= requiredFragments)
                    break;
            }
        }

        player.AddItem(fullKey);
    }
}