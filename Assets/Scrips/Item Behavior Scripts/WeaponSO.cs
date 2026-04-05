using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class WeaponSO : ScriptableObject
{
    [Header("Properties")]
    public float cooldown;
    public Sprite item_sprite;

    [Header("Prefab")]
    public GameObject itemPrefab; // what gets held in hand
}