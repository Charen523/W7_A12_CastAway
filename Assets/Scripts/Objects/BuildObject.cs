using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CraftSystem;

public class BuildObject : MonoBehaviour, IInteractable
{
    private List<Craft> crafts;
    private CraftSystem craftSystem;
    private Material plantMat;
    private GameObject oldFence;
    private GameObject oldShip;
    private UIInventory inventory;

    private void Start()
    {
        craftSystem = FindObjectOfType<CraftSystem>();
        if (craftSystem != null)
        {
            crafts = craftSystem.crafts; // CraftSystem에서 crafts 리스트 가져오기
            plantMat = craftSystem.transparentMat;
            oldFence = craftSystem.oldFence;
            oldShip = craftSystem.oldShip;
        }

        var player = CharacterManager.Instance.Player;
        if (player != null)
        {
            inventory = player.inventory;
        }
    }

    // 프롬프트 표시
    public void GetInteractPrompt()
    {
        foreach (var craft in crafts)
        {   // 아직 지어지지 않았으면,
            if (craft.tag == gameObject.tag && !craft.isBuilt && craft.previewPrefab != null)
            {
                // craft.previewPrefab의 모든 Renderer의 material을 plantMat으로 설정
                Renderer[] renderers = craft.previewPrefab.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    renderer.material = plantMat;
                }

                craftSystem.promptPanel.SetActive(true); 
                string str = $"[E]를 눌러 {craft.name} 건설하기\n재료:{craft.matInfo}";
                craftSystem.promptText.text = str; // 오브젝트 설명 표시

                // 10초 뒤에 ClosePrompt 실행
                StartCoroutine(ClosePromptAfterDelay(10.0f));
            }
            
        }
    }
    // 프롬프트 자동 비활성화 코루틴
    private IEnumerator ClosePromptAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClosePrompt();
        craftSystem.Initialize();
    }

    public void ClosePrompt()
    {
        craftSystem.promptPanel.SetActive(false);
    }

    public void OnInteract()
    {
        if (crafts == null || inventory == null)
        {
            Debug.LogError("초기화 미완료");
            return;
        }

        foreach (var craft in crafts)
        {
            if (gameObject.tag == craft.tag && !craft.isBuilt)
            {   // 재료가 모두 있으면,
                if (HasRequiredMaterials(craft))
                {   //건설
                    PlaceObject(craft);
                }
                else
                {
                    Debug.Log("필요한 재료가 부족합니다.");
                }
                break;
            }
        }
    }

    private bool HasRequiredMaterials(Craft craft)
    {
        if (inventory.slots == null)
        {
            Debug.LogError("인벤토리 아이템 없음");
            return false;
        }

        foreach (var material in craft.needMaterials)
        {
            int quantity = GetItemQuantity(material.matName);
            if (quantity < material.matNumber)
            {
                return false;
            }
        }
        return true;
    }

    private int GetItemQuantity(string itemName)
    {
        int totalQuantity = 0;
        foreach (var slot in inventory.slots)
        {
            if (slot.item != null && slot.item.itemId == itemName)
            {
                totalQuantity += slot.quantity;
            }
        }
        return totalQuantity;
    }

    private void PlaceObject(Craft craft)
    {
        craft.actualPrefab.SetActive(true);
        craft.isBuilt = true;
        Destroy(craft.previewPrefab); // 건설되면 프리뷰 프리팹 삭제
        // 집 건설 시 낡은 울타리 삭제
        if (craft.tag == "B0006")
        {
            Destroy(oldFence);
        }
        // 배 건설 시 낡은 배 삭제
        else if (craft.tag == "B0008")
        {
            Destroy(oldShip);
        }
        ClosePrompt(); // 상호작용 후 프롬프트 닫기
        RemoveUsedMaterials(craft);
    }

    private void RemoveUsedMaterials(Craft craft)
    {
        foreach (var material in craft.needMaterials)
        {
            RemoveItem(material.matName, material.matNumber);
        }
        inventory.UpdateUI();
    }

    private void RemoveItem(string itemName, int quantity)
    {
        foreach (var slot in inventory.slots)
        {
            if (slot.item != null && slot.item.itemId == itemName)
            {
                if (slot.quantity >= quantity)
                {
                    slot.quantity -= quantity;
                    if (slot.quantity <= 0)
                    {
                        slot.item = null;
                    }
                    return;
                }
                else
                {
                    quantity -= slot.quantity;
                    slot.item = null;
                    slot.quantity = 0;
                }
            }
        }
    }
}
