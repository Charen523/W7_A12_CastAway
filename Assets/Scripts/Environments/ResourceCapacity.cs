using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCapacity : MonoBehaviour
{
    // 자원 용량 설정
    public int maxHits = 5;
    public List<GameObject> respawnPrefabs;
    private int currentHits;

    private ResourcePoolManager poolManager;

    void Start()
    {
        currentHits = 0;
        poolManager = FindObjectOfType<ResourcePoolManager>();
    }

    private void Update()
    {
        // 각 프리팹에 대해 비활성화 상태인지 확인하고, 비활성화 상태면 재생성 호출
        foreach (var prefab in respawnPrefabs)
        {
            if (!prefab.activeSelf)
            {   // 오브젝트풀로 반환
                poolManager.ReturnObjectToPool(prefab);
                // 비활성화된 오브젝트 활성화 호출
                poolManager.ActivateInactiveObjects(prefab.tag);
            }
        }
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
}
