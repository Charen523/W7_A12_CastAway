using UnityEngine;

[System.Serializable]
public class RecipeMaterials
{
    public string itemId;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "SO/New Item", order = 0)]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemId;
    public eItemType itemType = eItemType.MATERIAL;
    public string displayName;
    public string description;
    public Sprite icon;

    [Header("Stacking")]
    public bool canStack = true;
    public int maxStackAmount = 99;

    [Header("Recipe")]
    public eCraftType craftType;
    public RecipeMaterials[] recipe;
    public int resultQuantity;
}