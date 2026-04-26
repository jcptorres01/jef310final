using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory Data")]
    public List<WeaponSO> inventoryList = new List<WeaponSO>();

    [Header("Starting Items")]
    public List<WeaponSO> startingItems = new List<WeaponSO>();

    [Header("Protected Items (Cannot be Removed)")]
    public List<WeaponSO> unremovableItems = new List<WeaponSO>();

    [Header("References")]
    [SerializeField] Camera cam;
    [SerializeField] Transform itemHolder; // where held items appear
    [SerializeField] GameObject pressToPickup;
    [SerializeField] Transform rayOrigin; // the object from which the ray will fire

    [Header("Inventory UI")]
    [SerializeField] Image[] inventorySlotImage;
    [SerializeField] Image[] inventoryBackgroundImage;
    [SerializeField] Sprite emptySlotSprite;

    public GameObject inventoryUI;
    
    public PlayerMovementBehavior player;

    private bool inventoryOpen = false;

    [Header("Settings")]
    public int playerReach = 3;
    public KeyCode throwItemKey = KeyCode.Q;
    public KeyCode pickUpItemKey = KeyCode.E;
    public KeyCode useItemKey = KeyCode.Mouse0;
    public KeyCode toggleInventoryKey = KeyCode.Q;

    private int selectedItem = 0;

    private List<GameObject> spawnedItems = new List<GameObject>();

    void Start()
    {
        inventoryList.Clear();

        List<WeaponSO> snapshot = new List<WeaponSO>(InventoryDataManager.Instance.savedInventory);

        foreach (WeaponSO item in snapshot)
        {
            AddItem(item);
        }

        if (InventoryDataManager.Instance.savedInventory.Count == 0)
        {
            foreach (WeaponSO item in startingItems)
            {
                AddItem(item);
                InventoryDataManager.Instance.AddItem(item);
            }
        }
    }

    void Update()
    {
        if (PauseMenu.isPaused == true)
            return;
        
        HandleInventoryToggle();

        if (inventoryOpen)
            return;

        HandleScrollInput();
        HandlePickup();
        HandleThrow();
        HandleUseItem();
        UpdateUI();

        // Draw debug ray in scene view
        Transform origin = rayOrigin != null ? rayOrigin : cam.transform;
        Debug.DrawRay(origin.position, origin.forward * playerReach, Color.red);
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
        Transform origin = rayOrigin != null ? rayOrigin : cam.transform;

        Ray ray = new Ray(origin.position, origin.forward);

        // Draw debug ray in the Scene view
        Debug.DrawRay(origin.position, origin.forward * playerReach, Color.red);

        // Perform SphereCastAll to get all hits along the ray
        RaycastHit[] hits = Physics.SphereCastAll(ray, rayRadius, playerReach);

        // Find the closest pickable item
        RaycastHit? closestItemHit = null;
        float closestDistance = Mathf.Infinity;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("InteractiveObject"))
            {
                float distance = Vector3.Distance(origin.position, hit.point);
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
                IInteract interactable = closestItemHit.Value.collider.GetComponent<IInteract>();
                if (interactable != null)
                {
                    interactable.Interacting();
                }
            }
        }
        else
        {
            pressToPickup.SetActive(false);
        }
    }

    void HandleUseItem()
    {
        //if (inventoryOpen)
            //return;

        if (Input.GetKeyDown(useItemKey) && inventoryList.Count > 0)
        {
            UseHeldItem();
        }
    }

    // ---------------- THROW ----------------
    void HandleThrow()
    {
        if (Input.GetKeyDown(throwItemKey) && inventoryList.Count > 0)
        {
            WeaponSO item = inventoryList[selectedItem];

            if (IsProtectedItem(item))
            {
                Debug.LogWarning(item.name + " cannot be removed or thrown away!");
                return;
            }

            // Remove from inventory
            inventoryList.RemoveAt(selectedItem);
            InventoryDataManager.Instance.RemoveItem(item);

            // Remove held object
            Destroy(spawnedItems[selectedItem]);
            spawnedItems.RemoveAt(selectedItem);

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

        // Save it globally
        InventoryDataManager.Instance.AddItem(item);

        GameObject newItem = Instantiate(item.itemPrefab, itemHolder);

        newItem.transform.localPosition = Vector3.zero;
        newItem.transform.localRotation = Quaternion.identity;

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
        /*
        for (int i = 0; i < inventoryBackgroundImage.Length; i++)
        {
            inventoryBackgroundImage[i].color =
                (i == selectedItem)
                ? new Color32(145, 255, 126, 255)
                : new Color32(219, 219, 219, 255);
        }
        */
    }

    void UseHeldItem()
    {
        if (spawnedItems.Count == 0) return;

        Debug.Log("Using item: " + inventoryList[selectedItem].name);
    }

    public bool HasItem(WeaponSO item)
    {
        return inventoryList.Contains(item);
    }

    void HandleInventoryToggle()
    {
        if (Input.GetKeyDown(toggleInventoryKey))
        {
            if (inventoryOpen)
                CloseInventory();
            else
                OpenInventory();
        }
    }

    void OpenInventory()
    {
        inventoryUI.SetActive(true);

        player.SetMovementFrozen(true);
        PauseMenu.escLocked = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        inventoryOpen = true;
    }

    void CloseInventory()
    {
        inventoryUI.SetActive(false);

        player.SetMovementFrozen(false);
        PauseMenu.escLocked = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inventoryOpen = false;
    }

    bool IsProtectedItem(WeaponSO item)
    {
        return unremovableItems.Contains(item);
    }
}