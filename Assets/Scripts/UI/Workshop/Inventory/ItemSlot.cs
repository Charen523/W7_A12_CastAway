using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public UIInventory inventory;

    [Header("Item Slot Info")]
    public int slotIndex;
    public ItemData item;
    private Button slotButton;

    private readonly string iconName = "Icon";
    private Image icon;

    private readonly string quantityName = "Quantity";
    private TextMeshProUGUI quatityText;
    public int quantity;

    private readonly string selectName = "Select";
    private Image selectEdge;
    private bool isSelected;

    private void Awake()
    {
        slotButton = GetComponent<Button>();

        icon = transform.Find(iconName).GetComponent<Image>();

        if (icon != null )
        {
            quatityText = icon.transform.Find(quantityName).GetComponent<TextMeshProUGUI>();
            selectEdge = icon.transform.Find(selectName).GetComponent<Image>();

            quantity = 0;
            isSelected = false;
        }
        else
        {
            Debug.LogWarning("icon 오브젝트를 찾을 수 없음!");
        }
    }

    private void Start()
    {
        slotButton.onClick.AddListener(OnClickSlot);
    }

    private void OnEnable()
    {
        selectEdge.enabled = isSelected;
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
    }

    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quatityText.text = string.Empty;
    }

    public void OnClickSlot()
    {
        inventory.SelectItem(slotIndex);
        isSelected = true;
        selectEdge.enabled = isSelected;
    }

    public void SelectedDisable()
    {
        isSelected = false;
        selectEdge.enabled = isSelected;
    }
}