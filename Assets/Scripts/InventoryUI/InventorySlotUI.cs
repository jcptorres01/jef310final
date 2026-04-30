using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour
{
    public int slotIndex;
    public PlayerInventory playerInventory;

    public Image icon;
    public InventoryItemReference reference;

    public void OnClick()
    {
        if (slotIndex < playerInventory.inventoryList.Count)
        {
            WeaponSO item = playerInventory.inventoryList[slotIndex];
            playerInventory.OnSlotClicked(item);
        }
    }

    public void SetItem(WeaponSO item)
    {
        if (item == null)
        {
            icon.sprite = null;
            if (reference != null) reference.weaponSO = null;
            return;
        }

        icon.sprite = item.item_sprite;

        if (reference != null)
            reference.weaponSO = item;
    }
}