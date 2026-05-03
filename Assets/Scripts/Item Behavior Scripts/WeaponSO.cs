using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponSO : ScriptableObject
{
    public string itemName;
    public Sprite item_sprite;
    public GameObject itemPrefab;

    public ItemType itemType;
}

public enum ItemType
{
    Camera,
    Generic,
    Weapon
}