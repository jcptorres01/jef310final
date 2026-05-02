using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [Header("UI")]
    public Image icon;

    [HideInInspector] public int slotIndex;
    [HideInInspector] public PlayerInventory playerInventory;

    public void SetItem(WeaponSO item)
    {
        if (item == null)
        {
            icon.sprite = null;
            icon.enabled = false;
            return;
        }

        icon.sprite = item.item_sprite;
        icon.enabled = true;
    }
}