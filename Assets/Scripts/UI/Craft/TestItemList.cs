using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
    public GameObject itemButtonPrefab;
    public Transform content;
    private string[] items = { "Sword", "Shield", "Potion", "Helmet", "Armor", "Boots", "Gloves", "Ring", "Amulet", "Bow", "Arrow" };

    void Start()
    {
        PopulateItemList();
    }

    void PopulateItemList()
    {
        foreach (string item in items)
        {
            GameObject button = Instantiate(itemButtonPrefab, content);
            button.GetComponentInChildren<TextMeshProUGUI>().text = item;
            button.GetComponent<Button>().onClick.AddListener(() => ShowItemRecipe(item));
        }

        AdjustContentHeight();
    }

    void ShowItemRecipe(string item)
    {
        // 여기에 아이템 레시피를 표시하는 로직을 추가합니다.
        Debug.Log("Showing recipe for " + item);
    }

    void AdjustContentHeight()
    {
        VerticalLayoutGroup layoutGroup = content.GetComponent<VerticalLayoutGroup>();
        RectTransform contentRect = content.GetComponent<RectTransform>();

        // 각 버튼의 높이와 버튼 사이의 간격을 계산하여 Content의 높이를 설정합니다.
        int itemCount = content.childCount;
        float itemHeight = layoutGroup.GetComponentInChildren<RectTransform>().rect.height;
        float spacing = layoutGroup.spacing;

        // 총 높이를 계산합니다.
        float totalHeight = itemHeight * itemCount + spacing * (itemCount - 1);

        // Content의 높이를 설정합니다.
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, totalHeight);
    }
}
