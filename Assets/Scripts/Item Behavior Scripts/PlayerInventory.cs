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

    private List<WeaponSO> selectedItems = new List<WeaponSO>();

    [Header("Settings")]
    public int playerReach = 3;
    public KeyCode throwItemKey = KeyCode.C;
    public KeyCode pickUpItemKey = KeyCode.E;
    public KeyCode useItemKey = KeyCode.Mouse0;
    public KeyCode toggleInventoryKey = KeyCode.Q;

    private int selectedItem = 0;

    private List<GameObject> spawnedItems = new List<GameObject>();

    // NEW: dispatcher hook (does not remove anything existing)
    public System.Action<WeaponSO> OnUseItem;

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

        Transform origin = rayOrigin != null ? rayOrigin : cam.transform;
        Debug.DrawRay(origin.position, origin.forward * playerReach, Color.red);
    }

    void HandleScrollInput()
    {
        if (inventoryList.Count == 0)
            return;

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

    void HandlePickup()
    {
        float rayRadius = 0.5f;
        Transform origin = rayOrigin != null ? rayOrigin : cam.transform;

        Ray ray = new Ray(origin.position, origin.forward);

        Debug.DrawRay(origin.position, origin.forward * playerReach, Color.red);

        RaycastHit[] hits = Physics.SphereCastAll(ray, rayRadius, playerReach);

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
        if (Input.GetKeyDown(useItemKey) && inventoryList.Count > 0)
        {
            WeaponSO currentItem = inventoryList[selectedItem];

            // NEW: external system trigger (does not remove any old logic structure)
            OnUseItem?.Invoke(currentItem);
        }
    }

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

            inventoryList.RemoveAt(selectedItem);
            InventoryDataManager.Instance.RemoveItem(item);

            Destroy(spawnedItems[selectedItem]);
            spawnedItems.RemoveAt(selectedItem);

            if (selectedItem >= inventoryList.Count)
                selectedItem = inventoryList.Count - 1;

            if (selectedItem < 0)
                selectedItem = 0;

            UpdateHeldItem();
        }
    }

    public void AddItem(WeaponSO item)
    {
        inventoryList.Add(item);

        InventoryDataManager.Instance.AddItem(item);

        GameObject newItem = Instantiate(item.itemPrefab, itemHolder);

        newItem.transform.localPosition = Vector3.zero;
        newItem.transform.localRotation = Quaternion.identity;

        newItem.SetActive(false);

        spawnedItems.Add(newItem);

        UpdateHeldItem();
    }

    void UpdateHeldItem()
    {
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            spawnedItems[i].SetActive(i == selectedItem);
        }
    }

    [SerializeField] private InventorySlotUI[] slots;

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventoryList.Count)
            {
                slots[i].SetItem(inventoryList[i]);
            }
            else
            {
                slots[i].SetItem(null);
            }
        }
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

    
    public bool HasItem(WeaponSO item)
    {
        return inventoryList.Contains(item);
    }

    // --------- Inventory UI Behavior --------------

    public void SwapItems(int indexA, int indexB)
    {
        WeaponSO temp = inventoryList[indexA];
        inventoryList[indexA] = inventoryList[indexB];
        inventoryList[indexB] = temp;

        Debug.Log("Button Pressed");

        UpdateUI();
    }

    public void OnSlotClicked(WeaponSO item)
    {
        if (selectedItems.Contains(item))
            selectedItems.Remove(item);
        else
            selectedItems.Add(item);

        CheckCrafting();
    }

    void CheckCrafting()
    {
        if (selectedItems.Count == 3 && AllAreFragments(selectedItems))
        {
            CraftKey();
        }
    }

    bool AllAreFragments(List<WeaponSO> items)
    {
        foreach (var item in items)
        {
            if (!item.name.Contains("KeyFragment_Item"))
                return false;
        }
        return true;
    }

    [Header("Crafted Key")]
    [SerializeField] private WeaponSO fullKeySO; // assign in Inspector

    void CraftKey()
    {
        foreach (var item in selectedItems)
        {
            inventoryList.Remove(item);
            InventoryDataManager.Instance.RemoveItem(item);
        }

        selectedItems.Clear();

        AddItem(fullKeySO);

        UpdateUI();
    }

    public void RemoveItem(WeaponSO item)
    {
        if (!inventoryList.Contains(item))
            return;

        int index = inventoryList.IndexOf(item);

        inventoryList.RemoveAt(index);
        InventoryDataManager.Instance.RemoveItem(item);

        if (index < spawnedItems.Count)
        {
            Destroy(spawnedItems[index]);
            spawnedItems.RemoveAt(index);
        }

        if (selectedItem >= inventoryList.Count)
            selectedItem = inventoryList.Count - 1;

        if (selectedItem < 0)
            selectedItem = 0;

        UpdateHeldItem();
        UpdateUI();
    }
}