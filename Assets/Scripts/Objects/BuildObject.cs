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


    private void Start()
    {
        craftSystem = FindObjectOfType<CraftSystem>();
        if (craftSystem != null)
        {
            crafts = craftSystem.crafts; // CraftSystem에서 crafts 리스트 가져오기
            plantMat = craftSystem.transparentMat;
            oldFence = craftSystem.oldFence;
        }
    }

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
                string str = $"[E]를 눌러 {craft.name} 건설하기\n재료: {craft.matInfo}";
                craftSystem.promptText.text = str; // 오브젝트 설명 표시

                // 10초 뒤에 ClosePrompt 실행
                StartCoroutine(ClosePromptAfterDelay(10.0f));
            }
            
        }
    }

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
        // 재료가 모두 있으면,
        PlaceObject();
    }

    void PlaceObject()
    {
        foreach (var craft in crafts)
        {
            if (gameObject.tag == craft.tag && !craft.isBuilt)
            {
                craft.actualPrefab.SetActive(true);
                craft.isBuilt = true;
                Destroy(craft.previewPrefab); // 건설되면 프리뷰 프리팹 삭제
                // 집 건설 시 낡은 울타리 삭제
                if(gameObject.tag == "B0006")
                {
                    Destroy(oldFence);
                    
                }
                ClosePrompt(); // 상호작용 후 프롬프트 닫기
                break;
            }
        }
    }
}
