using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResourceCapacity : MonoBehaviour, IInteractable
{
    // 자원 용량 설정
    public int maxHits = 5;
    public ItemData data;
    private int currentHits;
    int cropCount = 0;

    private ResourcePoolManager poolManager;

    void Start()
    {
        currentHits = 0;
        poolManager = FindObjectOfType<ResourcePoolManager>();
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

    // 플레이어와 충돌한 자원이 당근, 버섯인지 확인
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject prefab = gameObject;
            if (prefab.tag == "B1010"|| prefab.tag == "B1007" || prefab.tag == "B1008" || prefab.tag == "B1009"
                || prefab.tag == "B1001" || prefab.tag == "B1002")
            {
                poolManager.isCrop = true;
            }
        }
    }

    public void GetInteractPrompt()
    {
        //추후 주울 수 있는 아이템 옆에 [data.displayName] 띄워주기.
    }

    public void ClosePrompt()
    {
        //판넬 setactive false
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
