using System.Collections.Generic;
using UnityEngine;

public class InventoryDataManager : MonoBehaviour
{
    public static InventoryDataManager Instance;

    public List<WeaponSO> savedInventory = new List<WeaponSO>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(WeaponSO item)
    {
        savedInventory.Add(item);
    }

    public void RemoveItem(WeaponSO item)
    {
        savedInventory.Remove(item);
    }
}