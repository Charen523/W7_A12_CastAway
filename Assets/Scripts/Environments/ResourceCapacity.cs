using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using static CraftSystem;
using static ResourcePoolManager;

public class ResourceCapacity : MonoBehaviour, IInteractable
{
    // 자원 용량 설정
    public int maxHits = 5;
    public ItemData data;
    private int currentHits;
    int cropCount = 0;

    private ResourcePoolManager poolManager;
    public List<Pool> pools;
    private CraftSystem craftSystem;

    void Start()
    {
        currentHits = 0;
        poolManager = FindObjectOfType<ResourcePoolManager>();
        pools = poolManager.Pools; 
        craftSystem = FindObjectOfType<CraftSystem>();
    }

    // 도끼로 쳤을 때 호출되는 메서드
    public void Hit(GameObject prefab)
    {
        currentHits++;
        if (currentHits >= maxHits)
        {
            poolManager.ReturnObjectToPool(prefab);
            currentHits = 0;
        }
    }

    public void GetInteractPrompt()
    {
        GameObject prefab = gameObject;
        craftSystem.promptPanel.SetActive(true);
        foreach (var pool in pools)
        {

            if (prefab.tag == "B1002" && pool.tag == gameObject.tag)
            {
                string str = $"채집 가능 횟수: {3 - cropCount}/3\n[E]를 눌러 {pool.itemName} 얻기";
                craftSystem.promptText.text = str;
            }
            else if(prefab.tag != "B1002" && pool.tag == gameObject.tag)
            {
                string str = $"[E]를 눌러 {pool.itemName} 얻기";
                craftSystem.promptText.text = str;
            }

        }
            
        // 3초 뒤에 ClosePrompt 실행
        StartCoroutine(ClosePromptAfterDelay(3.0f));
    }

    private IEnumerator ClosePromptAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClosePrompt();
    }

    public void ClosePrompt()
    {
        craftSystem.promptPanel.SetActive(false);
    }

    public void OnInteract()
    {
        GameObject prefab = gameObject;

        //inventory에 아이템 넣기.
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();

        //덤불이면 3회 수집 후 풀로 반환
        if(prefab.tag == "B1002")
        {
            cropCount++;
            if (cropCount >= 3)
            {
                poolManager.ReturnObjectToPool(prefab);
                cropCount = 0;
            }
        }
        else
            poolManager.ReturnObjectToPool(prefab);
    }

    // 플레이어와 충돌한 자원이 동굴 앞 돌이면, currentHits >= maxHits 일 때, Destroy(gameObject)
}
