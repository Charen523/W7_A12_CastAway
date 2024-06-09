using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour
{
    public UICraft craft;

    [Header("Recipe Slot Info")]
    public int slotIndex;
    public ItemData itemData = null; //UICraft 제공.
    public eCraftType craftType;
    private Button recipeButton;

    /*RecipeUIs*/
    private readonly string iconName = "Icon";
    private readonly string iconName2 = "Icon2";
    private Image icon;
    private Image icon2;
    private readonly string titleName = "Title";
    private TextMeshProUGUI itemTitle;
    private readonly string edgeName = "Edge";
    private Image edge;

    /*Reciep Status*/
    private bool isSelected;

    private void Awake()
    {
        recipeButton = GetComponent<Button>();
        recipeButton.onClick.AddListener(OnClickRecipe);

        icon = transform.Find(iconName).GetComponent<Image>();
        icon2 = transform.Find(iconName2).GetComponent<Image>();
        itemTitle = transform.Find(titleName).GetComponent<TextMeshProUGUI>();
        edge = transform.Find(edgeName).GetComponent<Image>();
    }

    private void Start()
    {
        craftType = itemData.craftType;

        icon.sprite = itemData.icon;
        icon2.sprite = itemData.icon;
        itemTitle.text = itemData.displayName;
        edge.enabled = false;

        if (craftType != eCraftType.HAND)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        edge.enabled = isSelected;
    }

    private void OnClickRecipe()
    {
        craft.SelectRecipe(slotIndex);
        isSelected = true;
        edge.enabled = isSelected;
    }

    public void SelectedDisable()
    {
        isSelected = false;
        edge.enabled = isSelected;
    }
}