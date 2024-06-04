using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public UIInventory inventory;
   
    public ItemData item;
    
    public int index;
    public bool equipped;

    [Header("Item Slot Info")]
    private Button slotButton;

    [Header("Item Slot Children")]
    private Image icon;
    private TextMeshProUGUI quatityText;
    public int quantity;
    private GameObject Select;
    public bool Selected;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        Select.SetActive(Selected);
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if (Select.activeSelf)
        {
            Select.SetActive(Selected);
        }
    }

    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quatityText.text = string.Empty;
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}