using UnityEngine;

public class WeaponPickable : MonoBehaviour, IPickable
{
    public WeaponSO weaponScriptableObject;

    public void PickItem()
    {
        FindObjectOfType<PlayerInventory>().AddItem(weaponScriptableObject);
        Destroy(gameObject);
    }
}