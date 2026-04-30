using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotDrop : MonoBehaviour, IDropHandler
{
    public InventorySlotUI slot;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem dragged = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (dragged != null)
        {
            Transform draggedParent = dragged.transform.parent;

            // Swap parents
            dragged.transform.SetParent(transform);
        }
    }
}