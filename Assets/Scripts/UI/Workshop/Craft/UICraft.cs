using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UICraft : MonoBehaviour
{
    private enum eRecipeIndex
    {
        MATERIAL_NAME,
        CURRENT_VALUE,
        REQUIRE_VALUE

    }

    [Header("Craft")]
    public Transform content;//인스펙터창
    public GameObject recipeSlot;
    public List<TextMeshProUGUI> selectedRecipe; //인스펙터창
    public Image resultImg;
    public GameObject craftButton;
    public List<RecipeSlot> slots = new List<RecipeSlot>();
    private RecipeSlot selectedSlot;
    private bool isCraftable;
    private Dictionary<int, int> materialIndexes = new Dictionary<int, int>();

    [Header("Inventory")]
    public UIInventory inventory; //인스펙터창

    private void Awake()
    {
        foreach (var data in DataManager.Instance.itemDataDictionary)
        {
            if (data.Value.craftType != eCraftType.DEFAULT)
            {
                RecipeSlot newSlot = Instantiate(recipeSlot, content).GetComponent<RecipeSlot>();
                newSlot.craft = this;
                newSlot.itemData = data.Value;
                newSlot.craftType = data.Value.craftType;
                slots.Add(newSlot);
            }  
        }
    }

    private void Start()
    {
        craftButton.GetComponent<Button>().onClick.AddListener(OnCraftBtn);
        CharacterManager.Instance.Player.NearWorkBench += EnableWorkBenchRecipe;
        CharacterManager.Instance.Player.FarWorkBench += DisableWorkBenchRecipe;
        CharacterManager.Instance.Player.InHome += EnableFurnaceRecipe;
        CharacterManager.Instance.Player.OutHome += DisableFurnaceRecipe;

        for (int i = 0; i <  slots.Count; i++)
        {
            slots[i].slotIndex = i;
            if (slots[i].craftType != eCraftType.HAND) 
                slots[i].gameObject.SetActive(false);
        }

        ClearSelectedRecipeWindow();
    }

    private void OnCraftBtn()
    {
        foreach (var data in materialIndexes)
        {
            inventory.RemoveUsedItem(data.Key, data.Value);
        }
        inventory.AddItem(selectedSlot.itemData);
        ClearSelectedRecipeWindow();
    }

    private void ClearSelectedRecipeWindow()
    {
        selectedSlot = null;

        foreach (var element in selectedRecipe)
        {
            element.text = string.Empty;
        }

        resultImg.sprite = null;
        materialIndexes.Clear();
        craftButton.SetActive(false);
    }

    public void SelectRecipe(int index)
    {
        int materialCount;
        int currentSufficient = 0;

        if (slots[index].itemData == null) return;
        if (selectedSlot != null) { selectedSlot.SelectedDisable(); }

        selectedSlot = slots[index];

        selectedRecipe[(int)eRecipeIndex.MATERIAL_NAME].text = string.Empty;
        selectedRecipe[(int)eRecipeIndex.REQUIRE_VALUE].text = string.Empty;
        selectedRecipe[(int)eRecipeIndex.CURRENT_VALUE].text = string.Empty;

        materialCount = selectedSlot.itemData.recipe.Length;

        for (int i = 0; i < materialCount; i++)
        {
            string itemId = selectedSlot.itemData.recipe[i].itemId;
            ItemData currentItemData = DataManager.Instance.itemDataDictionary[itemId];
            string itemName = currentItemData.displayName;

            selectedRecipe[(int)eRecipeIndex.MATERIAL_NAME].text
                += itemName + " \n";
            selectedRecipe[(int)eRecipeIndex.REQUIRE_VALUE].text
                += selectedSlot.itemData.recipe[i].value + "\n";

            //인벤토리에서 이 아이템 찾기.
            ItemSlot findSlot = inventory.GetItemStack(currentItemData);
            if (findSlot == null)
            {
                selectedRecipe[(int)eRecipeIndex.CURRENT_VALUE].text += 0 + "\n";
            }
            else
            {
                selectedRecipe[(int)eRecipeIndex.CURRENT_VALUE].text += findSlot.quantity + "\n";

                // 중복된 키 추가 방지
                if (!materialIndexes.ContainsKey(findSlot.slotIndex))
                {
                    materialIndexes.Add(findSlot.slotIndex, selectedSlot.itemData.recipe[i].value);
                }

                if (findSlot.quantity >= selectedSlot.itemData.recipe[i].value)
                    currentSufficient++;
            }
        }

        if (currentSufficient >= materialCount)
        {
            isCraftable = true;
        }
        else
        {
            isCraftable = false;
        }

        if (isCraftable)
        {
            craftButton.SetActive(true);
        }
        else
        {
            craftButton.SetActive(false);
        }

    }

    private void EnableWorkBenchRecipe()
    {
        for (int i =0; i < slots.Count; i++)
        {
            if (slots[i].craftType == eCraftType.WORKBENCH)
            {
                slots[i].gameObject.SetActive(true);
            }
        }
    }

    private void DisableWorkBenchRecipe()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].craftType == eCraftType.WORKBENCH)
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    private void EnableFurnaceRecipe()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].craftType == eCraftType.FURNACE)
            {
                slots[i].gameObject.SetActive(true);
            }
        }
    }

    private void DisableFurnaceRecipe()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].craftType == eCraftType.FURNACE)
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }
}