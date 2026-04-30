using UnityEngine;
using UnityEngine.EventSystems;

public class CraftDropZone : MonoBehaviour, IDropHandler
{
    public Transform craftContainer;
    public CraftingSystem craftingSystem;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            Debug.LogWarning("Nothing was dragged into CraftZone.");
            return;
        }

        DraggableItem item = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (item == null)
        {
            Debug.LogWarning("Dragged object has no DraggableItem.");
            return;
        }

        item.transform.SetParent(craftContainer);
        item.transform.localPosition = Vector3.zero;

        item.droppedInCraftZone = true;

        // IMPORTANT: register item in crafting system
        craftingSystem.AddToCraftZone(item);
    }
}