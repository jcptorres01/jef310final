using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public List<WeaponSO> itemsInCraftZone = new List<WeaponSO>();

    public PlayerInventory inventory;
    public WeaponSO craftedKey;

    public void AddToCraftZone(DraggableItem item)
    {
        if (item == null)
        {
            Debug.LogWarning("Null DraggableItem ignored");
            return;
        }

        if (item.itemRef == null)
        {
            Debug.LogWarning("Item missing InventoryItemReference");
            return;
        }

        if (item.itemRef.weaponSO == null)
        {
            Debug.LogWarning("WeaponSO is missing on itemRef");
            return;
        }

        itemsInCraftZone.Add(item.itemRef.weaponSO);
    }

    public void TryCraft()
    {
        itemsInCraftZone.RemoveAll(i => i == null);

        if (itemsInCraftZone.Count != 3)
            return;

        if (!AllAreFragments())
            return;

        CraftKey();
    }

    bool AllAreFragments()
{
    foreach (var item in itemsInCraftZone)
    {
        if (item == null)
        {
            Debug.LogWarning("Null item found in craft zone list");
            continue;
        }

        if (!item.name.Contains("KeyFragment_Item"))
            return false;
    }

    return true;
}

    void CraftKey()
    {
        foreach (var item in itemsInCraftZone)
        {
            inventory.RemoveItem(item);
        }

        itemsInCraftZone.Clear();

        inventory.AddItem(craftedKey);
    }
}