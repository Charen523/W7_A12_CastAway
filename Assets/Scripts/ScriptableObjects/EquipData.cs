using UnityEngine;

[CreateAssetMenu(fileName = "EquipItem", menuName = "SO/New Equip", order = 2)]
public class EquipData : ItemData
{
    [Header("Equip")]
    public GameObject equipPrefab;

    private void OnEnable()
    {
        type = eItemType.EQUIP;
        canStack = false;
        maxStackAmount = 0;
    }
}