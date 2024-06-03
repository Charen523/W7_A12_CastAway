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
    public bool canStack = true;
    public int maxStackAmount = 99;
}
