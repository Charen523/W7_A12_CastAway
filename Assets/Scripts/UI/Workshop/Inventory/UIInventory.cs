using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class UIInventory : MonoBehaviour
{
    public bool initCheck = false;

    /*플레이어*/
    private PlayerCondition condition;
    private Transform dropPosition;

    [Header("Inventory")]
    private Transform holdings; //인벤 슬롯 묶음
    private ItemSlot[] slots; //슬롯 배열.
    private ItemSlot selectedSlot; //선택된 아이템슬롯.
    private int selectedSlotIndex; 
    public List<TextMeshProUGUI> selectedDescriptions;
    //TMP Index
    //[0]: selectedItemName
    //[1]: selectedItemDescription
    //[2]: selectedItemStatName
    //[3]: selectedItemStatValue
    public List<GameObject> invenBtns = new List<GameObject>();
    //Button Index
    //[0]: UseBtn
    //[1]: DropBtn

    //장비쪽으로 넘기기.
    //public GameObject equipButton;
    //public GameObject unEquipButton;

    private void Awake()
    {
        holdings = transform.Find("Holdings");
        
        foreach (Transform transform in transform.Find("InvenBtns"))
        {
            invenBtns.Add(transform.gameObject);
        }

        foreach (Transform transform in transform.Find("ItemInfo"))
        {
            if (transform.TryGetComponent(out TextMeshProUGUI component))
                selectedDescriptions.Add(component);
        }
    }

    void Start()
    {
        /*플레이어 스크립트 불러오기.*/
        condition = CharacterManager.Instance.Player.condition; //아이템 사용할 때.
        dropPosition = CharacterManager.Instance.Player.dropPosition; //아이템 버릴 때.

        CharacterManager.Instance.Player.addItem += AddItem; //아이템 습득 이벤트 등록.
        
        slots = new ItemSlot[holdings.childCount];

        /*slot 초기화.*/
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = holdings.GetChild(i).GetComponent<ItemSlot>();
            slots[i].inventory = this;
            slots[i].slotIndex = i;
            slots[i].Clear();
        }

        invenBtns[0].GetComponent<Button>().onClick.AddListener(OnUseButton);
        invenBtns[1].GetComponent<Button>().onClick.AddListener(OnDropButton);

        ClearSelectedItemWindow();
        UpdateUI();

        initCheck = true;
    }

    private void OnUseButton()
    {
        if (selectedSlot.item is ConsumableData consumeData)
        {
            for (int i = 0; i < consumeData.consumables.Length; i++)
            {
                switch (consumeData.consumables[i].type)
                {
                    case eConsumableType.HUNGER:
                        condition.Eat(consumeData.consumables[i].value);
                        break;
                    case eConsumableType.THIRST:
                        break;
                    case eConsumableType.HEALTH:
                        condition.Heal(consumeData.consumables[i].value);
                        break;
                    case eConsumableType.STAMINA:
                        //플레이어 컨디션 완료 후 추가 예정.
                        break;
                    case eConsumableType.TEMPERATURE:
                        //미구현.
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }

    private void OnDropButton()
    {
        ThrowItem(selectedSlot.item);
        RemoveSelectedItem();
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
        Instantiate(Resources.Load(($"Item_Prefabs/{data.itemId}")), dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;
        if (selectedSlot != null) { selectedSlot.SelectedDisable(); }
        
        selectedSlot = slots[index];

        selectedDescriptions[0].text = selectedSlot.item.displayName;
        selectedDescriptions[1].text = selectedSlot.item.description;
        selectedDescriptions[2].text = string.Empty;
        selectedDescriptions[3].text = string.Empty;

        if (selectedSlot.item is ConsumableData consumableItem)
        {
            for (int i = 0; i < consumableItem.consumables.Length; i++)
            {
                selectedDescriptions[2].text += consumableItem.consumables[i].type.ToString() + ":\n";
                selectedDescriptions[3].text += consumableItem.consumables[i].value.ToString() + "\n";
            }
            invenBtns[0].SetActive(true);
        }
        else if (invenBtns[0].activeSelf) //사용 불가능하면 버튼 비활성화.
        {
            invenBtns[0].SetActive(false);
        }

        invenBtns[1].SetActive(true);
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
}