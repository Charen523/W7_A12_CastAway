using UnityEngine;

[System.Serializable]
public class ItemDataEquippable
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
    public ItemDataEquippable[] equipStats;
    public bool isEquipped;

    private void OnEnable()
    {
        itemType = eItemType.EQUIP;
        canStack = false;
        maxStackAmount = 0;
    }

    [Header("Equip")]
    public GameObject equipPrefab; // 장착한 장비 프리펩
}