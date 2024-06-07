using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class UIInventory : MonoBehaviour
{
    private enum eDescriptionIndex
    {
        ITEM_NAME,
        ITEM_DESCRIPTION,
        STAT_NAME,
        STAT_VALUE
    }

    private enum eBtnIndex
    {
        USE_BTN,
        DROP_BTN,
        EQUIP_BTN,
        UNEQUIP_BTN
    }

    [Header("Inventory")]
    private Transform holdings;
    public List<TextMeshProUGUI> selectedDescriptions;
    public List<GameObject> invenBtns = new List<GameObject>();
    private ItemSlot[] slots;
    private ItemSlot selectedSlot;
    private int selectedSlotIndex; 
    
    /*플레이어*/
    private PlayerCondition condition;
    private Transform dropPosition;

    private void Awake()
    {
        holdings = transform.Find("Holdings");

        foreach (Transform transform in transform.Find("ItemInfo"))
        {
            if (transform.TryGetComponent(out TextMeshProUGUI component))
                selectedDescriptions.Add(component);
        }

        foreach (Transform transform in transform.Find("InvenBtns"))
        {
            invenBtns.Add(transform.gameObject);
        }
    }

    private void Start()
    {
        invenBtns[(int)eBtnIndex.USE_BTN].GetComponent<Button>().onClick.AddListener(OnUseBtn);
        invenBtns[(int)eBtnIndex.DROP_BTN].GetComponent<Button>().onClick.AddListener(OnDropBtn);
        invenBtns[(int)eBtnIndex.EQUIP_BTN].GetComponent<Button>().onClick.AddListener(OnEquipBtn);
        invenBtns[(int)eBtnIndex.UNEQUIP_BTN].GetComponent<Button>().onClick.AddListener(OnUnequipBtn);

        /*slot 초기화.*/
        slots = new ItemSlot[holdings.childCount];
        
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = holdings.GetChild(i).GetComponent<ItemSlot>();
            slots[i].inventory = this;
            slots[i].slotIndex = i;
            slots[i].Clear();
        }

        CharacterManager.Instance.Player.addItem += AddItem;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        ClearSelectedItemWindow();
        UpdateUI();
    }

    private void OnUseBtn()
    {
        if (selectedSlot.item is ConsumableData consumeData)
        {
            for (int i = 0; i < consumeData.consumables.Length; i++)
            {
                switch (consumeData.consumables[i].type)
                {
                    case eConditionType.HUNGER:
                        condition.Eat(consumeData.consumables[i].value);
                        break;
                    case eConditionType.THIRST:
                        condition.Drink(consumeData.consumables[i].value);
                        break;
                    case eConditionType.HEALTH:
                        condition.Heal(consumeData.consumables[i].value);
                        break;
                    case eConditionType.STAMINA:
                        condition.GiveEnergy(consumeData.consumables[i].value); 
                        break;
                    case eConditionType.TEMPERATURE:
                        //고추 아이템 추가 시 더워지게 등등? 아이디어만 있음.
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }

    private void OnDropBtn()
    {
        ThrowItem(selectedSlot.item);
        RemoveSelectedItem();
    }

    private void OnEquipBtn()
    {
        if (selectedSlot.item is EquipData equipData)
        {
            //def는 플레이어의 스탯이므로 관련 처리 필요.
            equipData.isEquipped = true;
            //equipType에 따른 장비 슬롯에 장착하는 상호작용 필요.
            //Playe.equip에서 장착했다고 알려야 함.
            //장착된 아이템임을 알리는 별도의 UI 필요.
            
            invenBtns[(int)eBtnIndex.EQUIP_BTN].SetActive(false);
            invenBtns[(int)eBtnIndex.UNEQUIP_BTN].SetActive(true);
        }
    }

    private void OnUnequipBtn()
    {
        if (selectedSlot.item is EquipData equipData)
        {
            //def 스탯 원복 등 위의 주석내용 반대의 작업 필요.
            equipData.isEquipped= false;

            invenBtns[(int)eBtnIndex.EQUIP_BTN].SetActive(true);
            invenBtns[(int)eBtnIndex.UNEQUIP_BTN].SetActive(false);
        }
    }

    private void ClearSelectedItemWindow()
    {
        selectedSlot = null;

        foreach (var element in selectedDescriptions)
        {
            element.text = string.Empty;
        }

        foreach (var element in invenBtns)
        {
            element.SetActive(false);
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;
        Debug.Log(data.name);

        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null) //stack이 가능하고 현재 인벤토리에 존재할 떄.
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null) //빈 슬롯이 존재할 떄.
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        ThrowItem(data); //인벤토리가 꽉 찼을 때.
        CharacterManager.Instance.Player.itemData = null;
    }

    public void ThrowItem(ItemData data)
    {
        Instantiate(DataManager.Instance.itemPrefabDictionary[data.name], dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;
        if (selectedSlot != null) { selectedSlot.SelectedDisable(); }
        
        selectedSlot = slots[index];

        selectedDescriptions[(int)eDescriptionIndex.ITEM_NAME].text = selectedSlot.item.displayName;
        selectedDescriptions[(int)eDescriptionIndex.ITEM_DESCRIPTION].text = selectedSlot.item.description;
        selectedDescriptions[(int)eDescriptionIndex.STAT_NAME].text = string.Empty;
        selectedDescriptions[(int)eDescriptionIndex.STAT_VALUE].text = string.Empty;

        if (selectedSlot.item is ConsumableData consumableItem)
        {
            for (int i = 0; i < consumableItem.consumables.Length; i++)
            {
                selectedDescriptions[(int)eDescriptionIndex.STAT_NAME].text 
                    += consumableItem.consumables[i].type.ToString() + " :\n";
                selectedDescriptions[(int)eDescriptionIndex.STAT_VALUE].text
                    += consumableItem.consumables[i].value.ToString() + "\n";
            }

            invenBtns[(int)eBtnIndex.USE_BTN].SetActive(true);
        }
        else
        {
            invenBtns[(int)eBtnIndex.USE_BTN].SetActive(false);

            if (selectedSlot.item is EquipData equipItem)
            {
                for (int i = 0; i < equipItem.equipStats.Length; i++)
                {
                    selectedDescriptions[(int)eDescriptionIndex.STAT_NAME].text 
                        += equipItem.equipStats[i].type.ToString() + ":\n";
                    selectedDescriptions[(int)eDescriptionIndex.STAT_VALUE].text 
                        += equipItem.equipStats[i].value.ToString() + "\n";
                }

                if (equipItem.isEquipped)
                {
                    invenBtns[(int)eBtnIndex.UNEQUIP_BTN].SetActive(true);
                }
                else
                {
                    invenBtns[(int)eBtnIndex.EQUIP_BTN].SetActive(true);
                }
            }
            else
            {
                invenBtns[(int)eBtnIndex.EQUIP_BTN].SetActive(false);
                invenBtns[(int)eBtnIndex.UNEQUIP_BTN].SetActive(false);
            }
        }

        invenBtns[(int)eBtnIndex.DROP_BTN].SetActive(true);
    }

    void RemoveSelectedItem()
    {
        selectedSlot.quantity--;

        if (selectedSlot.quantity <= 0)
        {
            selectedSlot.item = null;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }

    //인벤토리에 들어온 아이템을 쌓을 수 있는지 여부. 
    private ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    //비어있는 슬롯을 찾아주고 없으면 null 반환.
    private ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    public bool GetItemInInventory(string id, int quantity)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item.name == id && quantity <= slots[i].quantity)
            {
                return true;
            }
        }


        return false;
    }
}