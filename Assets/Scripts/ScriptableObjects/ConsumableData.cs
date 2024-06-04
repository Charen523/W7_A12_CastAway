using UnityEngine;

[System.Serializable]
public class ItemDataConsumable
{
    public eConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "ConsumableItem", menuName = "SO/New Consumable", order = 1)]
public class ConsumableData : ItemData
{
    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    private void OnEnable()
    {
        type = eItemType.CONSUME;
    }
}