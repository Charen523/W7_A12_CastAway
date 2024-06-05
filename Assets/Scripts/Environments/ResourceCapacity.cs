using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCapacity : MonoBehaviour
{
    // 자원 용량 설정
    public int maxHits = 5;
    public GameObject RespawnPrefab;
    private int currentHits;

    private ResourcePoolManager poolManager;

    void Start()
    {
        currentHits = 0;
        poolManager = FindObjectOfType<ResourcePoolManager>();
    }

    private void Update()
    {
        // 테스트용(비활성화 상태면 재생성 호출)
        if (!RespawnPrefab.activeSelf)
        {
            poolManager.ReturnObjectToPool(RespawnPrefab);
        }
    }

    // 도끼로 맞았을 때 호출되는 메서드
    public void Hit()
    {
        currentHits++;
        if (currentHits >= maxHits)
        {
            poolManager.ReturnObjectToPool(RespawnPrefab);
            currentHits = 0;
        }
    }
}
