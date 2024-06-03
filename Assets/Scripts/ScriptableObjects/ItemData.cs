using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "SO/New Item", order = 0)]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public eItemType type = eItemType.MATERIAL;
    public string displayName;
    public string description;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;
}

[System.Serializable]
public class ItemDataConsumable
{
    public eConsumableType type;  // 철자 확인 (eConSumableType -> eConsumableType)
    public float value;
}

[CreateAssetMenu(fileName = "ConsumableItem", menuName = "SO/New Consumable", order = 1)]
public class ConsumableData : ItemData
{
    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
}

[CreateAssetMenu(fileName = "EquipItem", menuName = "SO/New Equip", order = 2)]
public class EquipData : ItemData
{
    [Header("Equip")]
    public GameObject equipPrefab;
}
