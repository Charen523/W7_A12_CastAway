using UnityEngine;

[CreateAssetMenu(fileName = "EquipItem", menuName = "SO/New Equip", order = 2)]
public class EquipData : ItemData
{
    [Header("Equip")]
    public string EquipId = "E";

    private void OnEnable()
    {
        itemId = "I2";
        type = eItemType.EQUIP;
        canStack = false;
        maxStackAmount = 0;
    }
}