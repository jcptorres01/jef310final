using UnityEngine;

public class ItemUseDispatcher : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private ItemActions actions;

    void Awake()
    {
        inventory.OnUseItem += HandleUse;
    }

    void OnDestroy()
    {
        inventory.OnUseItem -= HandleUse;
    }

    void HandleUse(WeaponSO item)
    {
        if (item == null)
            return;

        switch (item.itemType)
        {
            case ItemType.Camera:
                actions.TakePicture();
                break;

            case ItemType.Weapon:
                actions.Attacking();
                break;

            default:
                Debug.Log("Used item: " + item.itemName);
                break;
        }
    }
}