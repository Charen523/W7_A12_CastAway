using UnityEngine;

[System.Serializable]
public class ItemDataEquipable
{
    public eEquipStatType type;
    public float value;
}

[CreateAssetMenu(fileName = "EquipItem", menuName = "SO/New Equip", order = 2)]
public class EquipData : ItemData
{
    [Header("Equip")]
    public string EquipId = "E";
    public eEquipType equipType;
    public ItemDataEquipable[] equipStats;
    public bool isEquipped;

    private void OnEnable()
    {
        itemId = "I2";
        type = eItemType.EQUIP;
        canStack = false;
        maxStackAmount = 0;
    }
}