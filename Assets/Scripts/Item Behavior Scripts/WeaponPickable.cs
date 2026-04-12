using UnityEngine;

public class WeaponPickable : MonoBehaviour, IInteract
{
    public WeaponSO weaponScriptableObject;

    public void Interacting()
    {
        FindObjectOfType<PlayerInventory>().AddItem(weaponScriptableObject);
        Destroy(gameObject);
    }
}