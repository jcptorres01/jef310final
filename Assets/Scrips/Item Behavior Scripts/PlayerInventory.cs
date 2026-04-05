using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory Data")]
    public List<WeaponSO> inventoryList = new List<WeaponSO>();

    [Header("References")]
    [SerializeField] Camera cam;
    [SerializeField] Transform itemHolder; // where held items appear
    [SerializeField] GameObject pressToPickup;

    [Header("UI")]
    [SerializeField] Image[] inventorySlotImage;
    [SerializeField] Image[] inventoryBackgroundImage;
    [SerializeField] Sprite emptySlotSprite;

    [Header("Settings")]
    public int playerReach = 3;
    public KeyCode throwItemKey = KeyCode.Q;
    public KeyCode pickUpItemKey = KeyCode.E;

    private int selectedItem = 0;

    private List<GameObject> spawnedItems = new List<GameObject>();

    void Update()
    {
        HandleScrollInput();
        HandlePickup();
        HandleThrow();
        UpdateUI();

        // Draw debug ray in scene view
        Debug.DrawRay(cam.transform.position, cam.transform.forward * playerReach, Color.red);
    }

    // ---------------- SCROLL SWITCH ----------------
    void HandleScrollInput()
    {
        if (inventoryList.Count == 0) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            selectedItem++;
            if (selectedItem >= inventoryList.Count)
                selectedItem = 0;

            UpdateHeldItem();
        }
        else if (scroll < 0f)
        {
            selectedItem--;
            if (selectedItem < 0)
                selectedItem = inventoryList.Count - 1;

            UpdateHeldItem();
        }
    }

    // ---------------- PICKUP ----------------
    void HandlePickup()
    {
        float rayRadius = 0.5f; // adjust this to make the area bigger
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        // Draw debug ray in the Scene view
        Debug.DrawRay(cam.transform.position, cam.transform.forward * playerReach, Color.red);

        // Perform SphereCastAll to get all hits along the ray
        RaycastHit[] hits = Physics.SphereCastAll(ray, rayRadius, playerReach);

        // Find the closest pickable item
        RaycastHit? closestItemHit = null;
        float closestDistance = Mathf.Infinity;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("ItemPickUp"))
            {
                float distance = Vector3.Distance(cam.transform.position, hit.point);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestItemHit = hit;
                }
            }
        }

        // Handle the closest pickable item if found
        if (closestItemHit.HasValue)
        {
            pressToPickup.SetActive(true);

            if (Input.GetKeyDown(pickUpItemKey))
            {
                IPickable pickable = closestItemHit.Value.collider.GetComponent<IPickable>();
                if (pickable != null)
                {
                    //inventoryList.Add(closestItemHit.Value.collider.GetComponent<WeaponPickable>().weaponScriptableObject.itemPrefab);
                    pickable.PickItem();
                }
            }
        }
        else
        {
            // No pickable item detected
            pressToPickup.SetActive(false);
        }
    }

    // ---------------- THROW ----------------
    void HandleThrow()
    {
        if (Input.GetKeyDown(throwItemKey) && inventoryList.Count > 0)
        {
            WeaponSO item = inventoryList[selectedItem];

            // Remove from inventory
            inventoryList.RemoveAt(selectedItem);

            // Remove held object
            Destroy(spawnedItems[selectedItem]);
            spawnedItems.RemoveAt(selectedItem);

            // Fix index
            if (selectedItem >= inventoryList.Count)
                selectedItem = inventoryList.Count - 1;

            if (selectedItem < 0)
                selectedItem = 0;

            UpdateHeldItem();
        }
    }

    // ---------------- ADD ITEM ----------------
    public void AddItem(WeaponSO item)
    {
        inventoryList.Add(item);

        // Spawn held object
        GameObject newItem = Instantiate(item.itemPrefab, itemHolder);
        newItem.SetActive(false);

        spawnedItems.Add(newItem);

        UpdateHeldItem();
    }

    // ---------------- EQUIP ITEM ----------------
    void UpdateHeldItem()
    {
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            spawnedItems[i].SetActive(i == selectedItem);
        }
    }

    // ---------------- UI ----------------
    void UpdateUI()
    {
        for (int i = 0; i < inventorySlotImage.Length; i++)
        {
            if (i < inventoryList.Count)
            {
                inventorySlotImage[i].sprite = inventoryList[i].item_sprite;
            }
            else
            {
                inventorySlotImage[i].sprite = emptySlotSprite;
            }
        }

        for (int i = 0; i < inventoryBackgroundImage.Length; i++)
        {
            inventoryBackgroundImage[i].color =
                (i == selectedItem)
                ? new Color32(145, 255, 126, 255)
                : new Color32(219, 219, 219, 255);
        }
    }
}